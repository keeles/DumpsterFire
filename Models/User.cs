namespace ASP.NETCore;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    // [Required]
    // public string Username { get; set; }
    // public string ProfilePicture { get; set; }
    // public string About { get; set; }
    // public int Rep { get; set; }
    // public ICollection<Thread> Threads { get; } = new List<Thread>();
    // public ICollection<Board> Boards { get; } = new List<Board>();
    // public ICollection<Post> Posts { get; } = new List<Post>();

    // public User(string username, string about, string profilePicture)
    // {
    //     Username = username;
    //     ProfilePicture = profilePicture;
    //     About = about;
    //     Rep = 0;
    // }
    public string? About { get; set; }
    public string? ProfilePicture { get; set; }
    public int Rep { get; set; } = 0;
}
