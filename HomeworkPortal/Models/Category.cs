using System.ComponentModel.DataAnnotations;

namespace HomeworkPortal.Models
{
    
    public class Category
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(250)]
        public string? Description { get; set; }
    }
}
