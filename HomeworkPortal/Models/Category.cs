using System.ComponentModel.DataAnnotations;

namespace HomeworkPortal.Models
{
    // Ödevlerin ait olduğu kategoriler (Matematik, Fizik, vb.)
    public class Category
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(250)]
        public string? Description { get; set; }
    }
}
