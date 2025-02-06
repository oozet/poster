public class Post
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedDate { get; set; }

    // public User User { get; set; }
    // public List<Reaction> Reactions { get; set; }
}
