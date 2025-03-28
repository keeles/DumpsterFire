namespace ASP.NETCore;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public string About { get; set; } = string.Empty;
    public string? ProfilePicture { get; set; }
    public int Rep { get; set; } = 0;

    public User() { }

    public User(string userName)
        : base(userName)
    {
        About = string.Empty;
        Rep = 0;
    }
}
