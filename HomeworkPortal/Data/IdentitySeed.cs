using HomeworkPortal.Models;
using Microsoft.AspNetCore.Identity;

namespace HomeworkPortal.Data
{
    public static class IdentitySeed
    {
        public static async Task SeedAsync(IServiceProvider sp)
        {
            var roleManager = sp.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Admin", "Teacher", "Student" };

            foreach (var r in roles)
            {
                if (!await roleManager.RoleExistsAsync(r))
                    await roleManager.CreateAsync(new IdentityRole<int>(r));
            }

            
            var adminEmail = "admin@test.com";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "1234");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            
            var teacherEmail = "teacher@test.com";
            var teacher = await userManager.FindByEmailAsync(teacherEmail);
            if (teacher == null)
            {
                teacher = new ApplicationUser
                {
                    UserName = "teacher",
                    Email = teacherEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(teacher, "1234");
                await userManager.AddToRoleAsync(teacher, "Teacher");
            }

            
            var studentEmail = "student@test.com";
            var student = await userManager.FindByEmailAsync(studentEmail);
            if (student == null)
            {
                student = new ApplicationUser
                {
                    UserName = "student",
                    Email = studentEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student, "1234");
                await userManager.AddToRoleAsync(student, "Student");
            }
        }
    }
}
