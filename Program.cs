using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "";

builder.Services.AddDbContext<PostDb>(options => options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "PosterAPI";
    config.Title = "PosterAPI v1";
    config.Version = "v1";
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "PosterAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.MapGet("/posts", async (PostDb db) => await db.Posts.ToListAsync());
app.MapGet(
    "/posts/{id}",
    async (Guid id, PostDb db) =>
        await db.Posts.FindAsync(id) is Post post ? Results.Ok(post) : Results.NotFound()
);

app.MapPost(
    "/posts",
    async (Post post, PostDb db) =>
    {
        db.Posts.Add(post);
        await db.SaveChangesAsync();
        return Results.Created($"/posts/{post.Id}", post);
    }
);

app.MapPut(
    "/posts/{id}",
    async (Guid id, Post inputPost, PostDb db) =>
    {
        var post = await db.Posts.FindAsync(id);

        if (post is null)
            return Results.NotFound();

        post.Title = inputPost.Title;
        post.Content = inputPost.Content;

        await db.SaveChangesAsync();
        return Results.NoContent();
    }
);

app.MapDelete(
    "/posts/{id}",
    async (Guid id, PostDb db) =>
    {
        if (await db.Posts.FindAsync(id) is Post post)
        {
            db.Posts.Remove(post);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }
        return Results.NotFound();
    }
);

app.Run();
