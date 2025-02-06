using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;
    public PostController()
    [HttpGet]
    public Task<IActionResult> GetAsync()
    {
        app.MapGet("/posts", async (PostDb db) => await db.Posts.ToListAsync());
        app.MapGet(
            "/posts/{id}",
            async (Guid id, PostDb db) =>
                await db.Posts.FindAsync(id) is Post post ? Results.Ok(post) : Results.NotFound()
        );
    }

    [HttpGet("{id}")]
    public IActionResult Get(Guid id)
    {
        app.MapGet("/posts", async (PostDb db) => await db.Posts.ToListAsync());
        app.MapGet(
            "/posts/{id}",
            async (Guid id, PostDb db) =>
                await db.Posts.FindAsync(id) is Post post ? Results.Ok(post) : Results.NotFound()
        );
    }

    [HttpPost]
    public IActionResult Post()
    {
        // Your code here
    }
}
