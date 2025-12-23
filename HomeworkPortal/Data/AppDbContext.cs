using HomeworkPortal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeworkPortal.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Assignment> Assignments { get; set; } = null!;
        public DbSet<Submission> Submissions { get; set; } = null!; 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Submission>()
                .HasOne(s => s.Assignment)
                .WithMany()
                .HasForeignKey(s => s.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Submission>()
                .HasOne(s => s.Student)
                .WithMany()
                .HasForeignKey(s => s.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Teacher)
                .WithMany()
                .HasForeignKey(a => a.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            
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
        }
    }
}
