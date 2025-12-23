using HomeworkPortal.Models;
using HomeworkPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

           
            ApplicationUser? user = await _userManager.FindByEmailAsync(model.EmailOrUserName);
            if (user == null)
                user = await _userManager.FindByNameAsync(model.EmailOrUserName);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı adı/e-posta veya şifre hatalı.");
                return View(model);
            }

           
            var result = await _signInManager.PasswordSignInAsync(
                user, model.Password, isPersistent: true, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Kullanıcı adı/e-posta veya şifre hatalı.");
                return View(model);
            }

            
            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("Index", "Admin");

            if (await _userManager.IsInRoleAsync(user, "Teacher"))
                return RedirectToAction("TeacherDashboard", "Assignment");

            if (await _userManager.IsInRoleAsync(user, "Student"))
                return RedirectToAction("Index", "Assignment");

            return RedirectToAction("Index", "Home");
        }

        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            
            var roleName = model.Role.ToString(); 
            if (string.IsNullOrWhiteSpace(roleName) || roleName == "0")
            {
                ModelState.AddModelError("", "Lütfen bir rol seçiniz.");
                return View(model);
            }

            
            var existingByEmail = await _userManager.FindByEmailAsync(model.Email);
            if (existingByEmail != null)
            {
                ModelState.AddModelError("", "Bu e-posta zaten kayıtlı.");
                return View(model);
            }

            var existingByUserName = await _userManager.FindByNameAsync(model.UserName);
            if (existingByUserName != null)
            {
                ModelState.AddModelError("", "Bu kullanıcı adı zaten alınmış.");
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = true
            };

            
            var createResult = await _userManager.CreateAsync(user, model.Password);

            if (!createResult.Succeeded)
            {
                foreach (var e in createResult.Errors)
                    ModelState.AddModelError("", e.Description);

                return View(model);
            }

            
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var roleCreate = await _roleManager.CreateAsync(new IdentityRole<int>(roleName));
                if (!roleCreate.Succeeded)
                {
                    foreach (var e in roleCreate.Errors)
                        ModelState.AddModelError("", e.Description);

                    return View(model);
                }
            }

            
            var addRoleResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!addRoleResult.Succeeded)
            {
                foreach (var e in addRoleResult.Errors)
                    ModelState.AddModelError("", e.Description);

                return View(model);
            }

            
            await _signInManager.SignInAsync(user, isPersistent: true);

            
            if (roleName == "Admin") return RedirectToAction("Index", "Admin");
            if (roleName == "Teacher") return RedirectToAction("TeacherDashboard", "Assignment");
            if (roleName == "Student") return RedirectToAction("Index", "Assignment");

            return RedirectToAction("Index", "Home");
        }

        
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
