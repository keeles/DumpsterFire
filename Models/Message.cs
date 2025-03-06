namespace ASP.NETCore;

using System.ComponentModel.DataAnnotations;

public class Message
{
    [Key]
    public int Serial { get; private set; }

    [Required]
    public string Content { get; set; }

    //TODO: Is this overriding?
    public DateTime CreatedAt { get; } = DateTime.Now;

    public Nullable<DateTime> ReadAt { get; set; }
    public User User { get; private set; }
    public User Recipient { get; private set; }

    public int ReplyingToMessageId { get; private set; }

    public Message() { }

    public Message(string content, User user, User recipient, int replyingToMessageId)
        : this()
    {
        Content = content;
        User = user;
        Recipient = recipient;
        ReplyingToMessageId = replyingToMessageId;
    }
}
