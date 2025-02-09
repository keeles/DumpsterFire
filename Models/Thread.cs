namespace ASP.NETCore;

using System.ComponentModel.DataAnnotations;

public class Thread
{
    [Key]
    public int Serial { get; private set; }

    [Required]
    public User User { get; private set; }
    public string Title { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Board Board { get; private set; }
    public ICollection<Post> Posts { get; } = new List<Post>();

    public Thread() { }

    public Thread(User user, string title, Board board)
        : this()
    {
        User = user;
        Title = title;
        Board = board;
    }
}
