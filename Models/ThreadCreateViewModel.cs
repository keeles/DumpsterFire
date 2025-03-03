using System.ComponentModel.DataAnnotations;

namespace ASP.NETCore.Models
{
    public class ThreadCreateViewModel
    {
        public int BoardId { get; set; }

        [Required(ErrorMessage = "Thread title is required")]
        [StringLength(
            100,
            MinimumLength = 3,
            ErrorMessage = "Title must be between 3 and 100 characters"
        )]
        [Display(Name = "Thread Title")]
        public string Title { get; set; }

        // Additional fields you might want to include
        public string BoardTitle { get; set; }
    }
}
