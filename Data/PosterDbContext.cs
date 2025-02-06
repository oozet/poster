using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

public class PosterDbContext : DbContext
{
    public PosterDbContext(DbContextOptions<PosterDbContext> options)
        : base(options) { }

    public DbSet<Post> Posts => Set<Post>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        try
        {
            base.OnModelCreating(builder);

            foreach (var entity in builder.Model.GetEntityTypes())
            {
                var tableName =
                    entity.GetTableName()
                    ?? throw new ArgumentNullException("Invalid string in table names.");

                entity.SetTableName(ToSnakeCase(tableName));

                foreach (var property in entity.GetProperties())
                {
                    var columnName =
                        property.GetColumnName()
                        ?? throw new ArgumentNullException("Invalid string in column names.");
                    property.SetColumnName(ToSnakeCase(columnName));
                }

                foreach (var key in entity.GetKeys())
                {
                    var name =
                        key.GetName()
                        ?? throw new ArgumentNullException("Invalid string in primary keys.");
                    ;
                    key.SetName(ToSnakeCase(name));
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    var name =
                        key.GetConstraintName()
                        ?? throw new ArgumentNullException("Invalid string in foreign keys.");
                    ;
                    key.SetConstraintName(ToSnakeCase(name));
                }

                foreach (var index in entity.GetIndexes())
                {
                    var name =
                        index.GetDatabaseName()
                        ?? throw new ArgumentNullException("Invalid string in database name.");
                    ;
                    index.SetDatabaseName(ToSnakeCase(name));
                }
            }
        }
        catch (ArgumentNullException ex)
        {
            Console.Error.WriteLine(ex.ToString());
        }
    }

    private string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var startUnderscores = Regex.Match(input, @"^_+").Value;
        return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}
