namespace ASP.NETCore;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;

public class Board
{
    [Key]
    public int Serial { get; private set; }

    [Required]
    public User User { get; private set; }
    public DateTime CreatedAt { get; } = DateTime.Now;
    public string Title { get; private set; }
    public ICollection<Thread> Threads { get; } = new List<Thread>();

    public Board() { }

    public Board(User user, string title)
        : this()
    {
        User = user;
        Title = title;
    }
}
