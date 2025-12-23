using HomeworkPortal.Models;
using HomeworkPortal.Repositories;
using HomeworkPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<Category> _categoryRepo;
        private readonly IRepository<Assignment> _assignmentRepo;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            IRepository<Category> categoryRepo,
            IRepository<Assignment> assignmentRepo)
        {
            _userManager = userManager;
            _categoryRepo = categoryRepo;
            _assignmentRepo = assignmentRepo;
        }

        
        public async Task<IActionResult> Index()
        {
            var userCount = _userManager.Users.Count();

            var categories = await _categoryRepo.GetAllAsync();
            var assignments = await _assignmentRepo.GetAllAsync();

            ViewBag.TotalUsers = userCount;
            ViewBag.TotalCategories = categories.Count;
            ViewBag.TotalAssignments = assignments.Count;

            
            var last = assignments
                .OrderByDescending(a => a.CreatedAt)
                .FirstOrDefault();

            if (last != null)
            {
                ViewBag.LastTitle = last.Title;
                ViewBag.LastCreatedAt = last.CreatedAt.ToString("dd.MM.yyyy HH:mm");
                ViewBag.LastDueDate = last.DueDate.HasValue
                    ? last.DueDate.Value.ToString("dd.MM.yyyy HH:mm")
                    : "-";
            }
            else
            {
                ViewBag.LastTitle = "-";
                ViewBag.LastCreatedAt = "-";
                ViewBag.LastDueDate = "-";
            }

            return View();
        }

        
        public async Task<IActionResult> Users()
        {
            var users = _userManager.Users.ToList();
            var vm = new List<UserListItemViewModel>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);

                vm.Add(new UserListItemViewModel
                {
                    Id = u.Id,
                    UserName = u.UserName ?? "",
                    Email = u.Email ?? "",
                    Role = roles.FirstOrDefault() ?? "-"
                });
            }

            return View(vm);
        }

        
        [HttpGet]
        public async Task<IActionResult> Categories()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return View(categories.OrderBy(c => c.Id).ToList());
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryRepo.GetAllAsync();
                return View("Categories", categories.OrderBy(c => c.Id).ToList());
            }

            await _categoryRepo.AddAsync(model);
            await _categoryRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Categories));
        }

        
        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(Category model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _categoryRepo.Update(model);
            await _categoryRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Categories));
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null) return NotFound();

            _categoryRepo.Remove(category);
            await _categoryRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Categories));
        }
    }
}
