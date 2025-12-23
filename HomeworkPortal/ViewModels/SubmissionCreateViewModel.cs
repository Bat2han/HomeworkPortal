using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HomeworkPortal.ViewModels
{
    public class SubmissionCreateViewModel
    {
        [Required]
        public int AssignmentId { get; set; }

        [Required]
        public IFormFile File { get; set; } = null!;

        [MaxLength(500)]
        public string? Note { get; set; }
    }
}
