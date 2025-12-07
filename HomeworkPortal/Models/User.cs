using System.ComponentModel.DataAnnotations;

namespace HomeworkPortal.Models
{
    // Sisteme giriş yapan kullanıcılar tablosu
    public class User
    {
        public int Id { get; set; }   // Primary key

        [Required, MaxLength(50)]
        public string UserName { get; set; } = null!;

        [Required, MaxLength(100)]
        public string Email { get; set; } = null!;

        // Şimdilik sade tutuyoruz: düz metin şifre
        // (Gerçek projede hash kullanılmalı)
        [Required]
        public string PasswordHash { get; set; } = null!;

        [Required]
        public UserRole Role { get; set; }  // Admin / Teacher / Student
    }
}
