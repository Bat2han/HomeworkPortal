using System.Security.Claims;
using HomeworkPortal.Hubs;
using HomeworkPortal.Models;
using HomeworkPortal.Repositories;
using HomeworkPortal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HomeworkPortal.Controllers
{
    [Authorize]
    public class AssignmentController : Controller
    {
        private readonly IRepository<Assignment> _assignmentRepo;
        private readonly IRepository<Category> _categoryRepo;
        private readonly IHubContext<NotificationHub> _hub;

        public AssignmentController(
            IRepository<Assignment> assignmentRepo,
            IRepository<Category> categoryRepo,
            IHubContext<NotificationHub> hub)
        {
            _assignmentRepo = assignmentRepo;
            _categoryRepo = categoryRepo;
            _hub = hub;
        }

        
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Index()
        {
            var assignments = await _assignmentRepo.GetAllAsync();
            var categories = await _categoryRepo.GetAllAsync();

            var categoryMap = categories.ToDictionary(c => c.Id, c => c.Name);

            var vm = assignments
                .OrderByDescending(a => a.CreatedAt)
                .Select(a => new AssignmentListItemViewModel
                {
                    Id = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    CreatedAt = a.CreatedAt,
                    DueDate = a.DueDate,
                    CategoryId = a.CategoryId,
                    CategoryName = categoryMap.TryGetValue(a.CategoryId, out var name) ? name : "Kategori Yok",
                    TeacherId = a.TeacherId
                })
                .ToList();

            return View(vm);
        }

        
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> TeacherDashboard()
        {
            int teacherId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var myAssignments = await _assignmentRepo.FindAsync(a => a.TeacherId == teacherId);

            var categories = await _categoryRepo.GetAllAsync();
            ViewBag.Categories = categories;

            return View(myAssignments);
        }

       
        [Authorize(Roles = "Teacher")]
        [HttpGet]
        public async Task<IActionResult> CreatePartial()
        {
            var categories = await _categoryRepo.GetAllAsync();
            ViewBag.Categories = categories;

            return PartialView("_CreateAssignmentPartial");
        }

        
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
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

            
            string dueDateStr = assignment.DueDate.HasValue
                ? assignment.DueDate.Value.ToString("dd.MM.yyyy HH:mm")
                : "-";

            
            await _hub.Clients.Group("Admins").SendAsync("NewAssignment", new
            {
                title = assignment.Title,
                teacher = User.Identity?.Name ?? "Teacher",
                dueDate = dueDateStr,
                createdAt = assignment.CreatedAt.ToString("dd.MM.yyyy HH:mm")
            });

            return Json(new { success = true });
        }

        
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            int teacherId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var assignment = await _assignmentRepo.GetByIdAsync(id);
            if (assignment == null)
                return NotFound();

            
            if (assignment.TeacherId != teacherId)
                return Forbid();

            _assignmentRepo.Remove(assignment);
            await _assignmentRepo.SaveChangesAsync();

            
            await _hub.Clients.Group("Admins").SendAsync("AssignmentDeleted", new
            {
                id = assignment.Id,
                title = assignment.Title,
                teacher = User.Identity?.Name ?? "Teacher",
                deletedAt = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
            });

            return RedirectToAction(nameof(TeacherDashboard));
        }
    }
}
