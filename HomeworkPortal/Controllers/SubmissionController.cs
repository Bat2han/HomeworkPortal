using System.Security.Claims;
using HomeworkPortal.Models;
using HomeworkPortal.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeworkPortal.Controllers
{
    [Authorize]
    public class SubmissionController : Controller
    {
        private readonly IRepository<Submission> _submissionRepo;
        private readonly IRepository<Assignment> _assignmentRepo;

        public SubmissionController(
            IRepository<Submission> submissionRepo,
            IRepository<Assignment> assignmentRepo)
        {
            _submissionRepo = submissionRepo;
            _assignmentRepo = assignmentRepo;
        }

        
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> My()
        {
            int studentId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var list = await _submissionRepo.FindAsync(s => s.StudentId == studentId);

            
            var ordered = list.OrderByDescending(s => s.UploadedAt).ToList();

            return View(ordered);
        }

        
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> ByAssignment(int assignmentId)
        {
            var subs = await _submissionRepo.FindAsync(s => s.AssignmentId == assignmentId);
            var ordered = subs.OrderByDescending(s => s.UploadedAt).ToList();

            ViewBag.AssignmentId = assignmentId;
            return View(ordered);
        }
    }
}
