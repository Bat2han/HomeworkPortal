using System.ComponentModel.DataAnnotations;
using HomeworkPortal.Models;

namespace HomeworkPortal.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunlu")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "E-posta zorunlu")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta girin")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Şifre zorunlu")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Şifre tekrar zorunlu")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Şifreler uyuşmuyor")]
        public string ConfirmPassword { get; set; } = null!;

        
        [Required(ErrorMessage = "Rol seçiniz")]
        [Range(1, 3, ErrorMessage = "Rol seçiniz")]
        public UserRole Role { get; set; }
    }
}
