using System.ComponentModel.DataAnnotations;

namespace HomeworkPortal.Models
{
    // Öğretmenlerin verdiği ödevler
    public class Assignment
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; }  // Ödevin verildiği tarih

        public DateTime? DueDate { get; set; }   // Teslim tarihi (opsiyonel)

        // İlişkiler:
        // Hangi öğretmen verdi?
        public int TeacherId { get; set; }
        public User Teacher { get; set; } = null!;

        // Hangi kategoriye ait?
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
