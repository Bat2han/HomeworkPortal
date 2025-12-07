using System.ComponentModel.DataAnnotations;
using HomeworkPortal.Models;

namespace HomeworkPortal.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required]
        public UserRole Role { get; set; }  // Admin/Teacher/Student seçimi
    }
}
