using HomeworkPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeworkPortal.Data
{
    // EF Core'un SQL Server ile konuşacağı ana sınıf
    public class ApplicationDbContext : DbContext
    {
        // Constructor: dışarıdan DbContextOptions alıyor (conn string vs.)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DB'deki tablolar
        public DbSet<User> Users => Set<User>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Assignment> Assignments => Set<Assignment>();

        // Seed data ve ilişkiler
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // İlişki: Assignment - Teacher
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Teacher)
                .WithMany()
                .HasForeignKey(a => a.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            // 8 kategori seed (ara sınavda "en az 8 kategori" şartını da hazırlar)
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Matematik", Description = "Matematik ödevleri" },
                new Category { Id = 2, Name = "Fizik", Description = "Fizik ödevleri" },
                new Category { Id = 3, Name = "Kimya", Description = "Kimya ödevleri" },
                new Category { Id = 4, Name = "Biyoloji", Description = "Biyoloji ödevleri" },
                new Category { Id = 5, Name = "Programlama", Description = "C#, Java, Python vb." },
                new Category { Id = 6, Name = "Veritabanı", Description = "SQL ve DB tasarımı" },
                new Category { Id = 7, Name = "Web Tasarımı", Description = "HTML, CSS, JS" },
                new Category { Id = 8, Name = "Projeler", Description = "Dönem projeleri" }
            );

            // 3 örnek kullanıcı seed (admin, öğretmen, öğrenci)
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, UserName = "admin", Email = "admin@test.com", PasswordHash = "1234", Role = UserRole.Admin },
                new User { Id = 2, UserName = "teacher", Email = "teacher@test.com", PasswordHash = "1234", Role = UserRole.Teacher },
                new User { Id = 3, UserName = "student", Email = "student@test.com", PasswordHash = "1234", Role = UserRole.Student }
            );
        }
    }
}
