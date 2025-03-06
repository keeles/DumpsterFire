using System.ComponentModel.DataAnnotations;

namespace ASP.NETCore.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "About is required")]
        public string About { get; set; }
        public string ProfilePicture { get; set; }
    }
}
