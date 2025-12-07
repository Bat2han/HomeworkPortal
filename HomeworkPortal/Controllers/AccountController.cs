using System.Security.Claims;
using HomeworkPortal.Models;
using HomeworkPortal.Repositories;
using HomeworkPortal.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRepository<User> _userRepo;

        public AccountController(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var users = await _userRepo.FindAsync(u =>
                (u.Email == model.EmailOrUserName || u.UserName == model.EmailOrUserName)
                && u.PasswordHash == model.Password);

            var user = users.FirstOrDefault();

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı adı/e-posta veya şifre hatalı.");
                return View(model);
            }

            await SignInUser(user);

            // 🔽 Rolüne göre yönlendirme
            return user.Role switch
            {
                UserRole.Admin => RedirectToAction("Index", "Admin"),
                UserRole.Teacher => RedirectToAction("TeacherDashboard", "Assignment"),
                UserRole.Student => RedirectToAction("Index", "Assignment"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Aynı email varsa reddet
            var exists = await _userRepo.FindAsync(u => u.Email == model.Email);
            if (exists.Any())
            {
                ModelState.AddModelError("Email", "Bu e-posta zaten kayıtlı.");
                return View(model);
            }

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                PasswordHash = model.Password, // DEMO: düz yazıyoruz
                Role = model.Role
            };

            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();

            // İstersen direkt giriş yapabiliriz:
            await SignInUser(user);

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }

        // /Account/Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // Cookie'ye claim'leri yazan yardımcı metod
        private async Task SignInUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                // Rolü string olarak cookie'ye yazıyoruz
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal);
        }
    }
}
