using System.Security.Claims;
using HomeworkPortal.Models;
using HomeworkPortal.Repositories;
using HomeworkPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkPortal.Controllers
{
    [Authorize] // Sadece giriş yapmış kullanıcılar
    public class AssignmentController : Controller
    {
        private readonly IRepository<Assignment> _assignmentRepo;
        private readonly IRepository<Category> _categoryRepo;

        public AssignmentController(IRepository<Assignment> assignmentRepo,
                                    IRepository<Category> categoryRepo)
        {
            _assignmentRepo = assignmentRepo;
            _categoryRepo = categoryRepo;
        }

        // (İleride tüm ödevleri göstermek için kullanabiliriz)
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var assignments = await _assignmentRepo.GetAllAsync();
            var categories = await _categoryRepo.GetAllAsync();
            ViewBag.Categories = categories;
            return View(assignments);
        }

        // 🧑‍🏫 Öğretmen paneli
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> TeacherDashboard()
        {
            int teacherId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Sadece oturumdaki öğretmenin verdiği ödevler
            var myAssignments = await _assignmentRepo.FindAsync(a => a.TeacherId == teacherId);

            var categories = await _categoryRepo.GetAllAsync();
            ViewBag.Categories = categories;

            return View(myAssignments);
        }

        // 🧩 AJAX ile yüklenecek partial form (GET)
        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public async Task<IActionResult> CreatePartial()
        {
            var categories = await _categoryRepo.GetAllAsync();
            ViewBag.Categories = categories;

            return PartialView("_CreateAssignmentPartial");
        }

        // 🧩 AJAX POST: yeni ödev oluşturur
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public async Task<IActionResult> CreateAjax(AssignmentCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryRepo.GetAllAsync();
                ViewBag.Categories = categories;
                return PartialView("_CreateAssignmentPartial", model);
            }

            int teacherId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var assignment = new Assignment
            {
                Title = model.Title,
                Description = model.Description,
                CreatedAt = DateTime.Now,
                DueDate = model.DueDate,
                CategoryId = model.CategoryId,
                TeacherId = teacherId
            };

            await _assignmentRepo.AddAsync(assignment);
            await _assignmentRepo.SaveChangesAsync();

            // AJAX'a basit bir başarılı sonucu JSON olarak dönüyoruz
            return Json(new { success = true });
        }
    }
}
