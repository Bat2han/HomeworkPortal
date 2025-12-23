using System.ComponentModel.DataAnnotations;

namespace HomeworkPortal.Models
{
    public enum SubmissionStatus
    {
        Submitted = 1,
        Late = 2
    }

    public class Submission
    {
        public int Id { get; set; }

        [Required]
        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; } = null!;

        [Required]
        public int StudentId { get; set; }
        public ApplicationUser Student { get; set; } = null!;

        [Required, MaxLength(260)]
        public string FilePath { get; set; } = null!; 

        [Required, MaxLength(260)]
        public string OriginalFileName { get; set; } = null!;

        
        public DateTime UploadedAt { get; set; } = DateTime.Now;

        
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public DateTime SubmittedAt => UploadedAt;

        [MaxLength(500)]
        public string? Note { get; set; }

        public SubmissionStatus Status { get; set; } = SubmissionStatus.Submitted;
    }
}
