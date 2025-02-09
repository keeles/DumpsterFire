namespace ASP.NETCore;

using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int Serial { get; private set; }

    [Required]
    public string Username { get; set; }
    public string Password { get; set; }
    public string ProfilePicture { get; set; }
    public string About { get; set; }
    public int Rep { get; set; }
    public ICollection<Thread> Threads { get; } = new List<Thread>();
    public ICollection<Board> Boards { get; } = new List<Board>();
    public ICollection<Post> Posts { get; } = new List<Post>();

    public User(string username, string password, string profilePicture, string about)
    {
        Username = username;
        Password = password;
        ProfilePicture = profilePicture;
        About = about;
        Rep = 0;
    }
}
