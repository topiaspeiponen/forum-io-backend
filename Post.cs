public class Post {
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? CreatorName { get; set; }
    public IList<Comment>? Comments { get; } = new List<Comment>();
}

public class Comment {
    public int Id { get; set; }
    public string? CreatorName { get; set; }
    public string? Content { get; set; }
    public int PostId { get; set; }
}