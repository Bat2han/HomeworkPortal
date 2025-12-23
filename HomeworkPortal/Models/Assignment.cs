using System.ComponentModel.DataAnnotations;
using HomeworkPortal.Models;


namespace HomeworkPortal.Models
{
   
    public class Assignment
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; }  

        public DateTime? DueDate { get; set; }   

       
        public int TeacherId { get; set; }
        public ApplicationUser? Teacher { get; set; }  


        
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
