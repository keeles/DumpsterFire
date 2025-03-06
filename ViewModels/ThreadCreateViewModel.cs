using System.ComponentModel.DataAnnotations;

namespace ASP.NETCore.Models
{
    public class ThreadCreateViewModel
    {
        public Board Board { get; set; }

        [Required(ErrorMessage = "Thread title is required")]
        [StringLength(
            100,
            MinimumLength = 3,
            ErrorMessage = "Title must be between 3 and 100 characters"
        )]
        [Display(Name = "Thread Title")]
        public string Title { get; set; }

        public ThreadCreateViewModel(Board board)
        {
            Board = board;
        }
    }
}
