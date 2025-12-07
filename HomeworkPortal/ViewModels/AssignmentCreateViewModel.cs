using System.ComponentModel.DataAnnotations;

namespace HomeworkPortal.ViewModels
{
    public class AssignmentCreateViewModel
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        public DateTime? DueDate { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
