namespace ASP.NETCore;

using System.ComponentModel.DataAnnotations;

public class Post
{
    [Key]
    public int Serial { get; private set; }

    [Required]
    public string Content { get; set; }

    //TODO: Is this overriding?
    public DateTime CreatedAt { get; } = DateTime.Now;
    public User User { get; private set; }
    public Thread Thread { get; private set; }

    public int? ReplyingToPostId { get; set; }

    public Post() { }

    public Post(string content, User user, Thread thread, int? replyingToId = null)
        : this()
    {
        Content = content;
        User = user;
        Thread = thread;
        ReplyingToPostId = replyingToId;
    }
}
