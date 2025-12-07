using HomeworkPortal.Models;
using HomeworkPortal.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkPortal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IRepository<User> _users;
        private readonly IRepository<Category> _categories;

        public AdminController(IRepository<User> users, IRepository<Category> categories)
        {
            _users = users;
            _categories = categories;
        }

        // /Admin
        public async Task<IActionResult> Index()
        {
            var userList = await _users.GetAllAsync();
            var categoryList = await _categories.GetAllAsync();

            ViewBag.UserCount = userList.Count;
            ViewBag.CategoryCount = categoryList.Count;

            return View();
        }

        // /Admin/Users  → tüm kullanıcılar
        public async Task<IActionResult> Users()
        {
            var userList = await _users.GetAllAsync();
            return View(userList);
        }

        // /Admin/Categories → tüm kategoriler
        public async Task<IActionResult> Categories()
        {
            var categoryList = await _categories.GetAllAsync();
            return View(categoryList);
        }

        // /Admin/CreateCategory (GET)
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View(new Category());
        }

        // /Admin/CreateCategory (POST)
        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _categories.AddAsync(model);
            await _categories.SaveChangesAsync();

            return RedirectToAction(nameof(Categories));
        }

        // /Admin/EditCategory/5 (GET)
        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _categories.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        // /Admin/EditCategory/5 (POST)
        [HttpPost]
        public async Task<IActionResult> EditCategory(Category model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _categories.Update(model);
            await _categories.SaveChangesAsync();

            return RedirectToAction(nameof(Categories));
        }

        // /Admin/DeleteCategory/5 (POST)
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categories.GetByIdAsync(id);
            if (category == null)
                return NotFound();

            _categories.Remove(category);
            await _categories.SaveChangesAsync();

            return RedirectToAction(nameof(Categories));
        }
    }
}
