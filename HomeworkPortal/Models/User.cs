using System.ComponentModel.DataAnnotations;

namespace HomeworkPortal.Models
{
    
    public class User
    {
        public int Id { get; set; }   

        [Required, MaxLength(50)]
        public string UserName { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Email { get; set; } = null!;

        
        
        [Required]
        public string PasswordHash { get; set; } = null!;

        [Required]
        public UserRole Role { get; set; }  
    }
}
