using Microsoft.AspNetCore.Mvc;
using VR_Labs_for_Higher_Education.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using VR_Labs_for_Higher_Education.Services;

namespace VR_Labs_for_Higher_Education.Controllers
{
    [Authorize(Roles = "Student")]
    [ApiController]
    [Route("[controller]")]
    public class StudentController : Controller
    {
        private readonly StudentService _studentService;
        private readonly ILogger<StudentController> _logger;

        public StudentController(StudentService studentService, ILogger<StudentController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        // Default redirect will be to the homepage
        [HttpGet]
        public IActionResult DefaultRedirect()
        {
            return RedirectToAction("StudentHomePage", "Student");
        }

        // Redirect to student home page
        [HttpGet("StudentHomePage")]
        public IActionResult StudentHomePage()
        {
            return View();
        }

        // Redirect to student profile page
        [HttpGet("StudentProfilePage")]
        public async Task<IActionResult> StudentProfilePage()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (!string.IsNullOrEmpty(userEmail))
            {
                var student = await _studentService.FindByEmailAsync(userEmail);

                if (student != null)
                {
                    // Retrieve the student's LabProgress data from your data source (e.g., database)
                    var labProgresses = student.LabProgress;

                    // Create a view model to pass LabProgress data to the view
                    var viewModel = new StudentProfile
                    {
                        Student = student,
                        LabProgresses = labProgresses
                    };

                    return View(viewModel);
                }
            }
            return NotFound();
        }

        // Redirect to lab preview page
        [HttpGet("StudentLabPage/{id}")]
        public async Task<IActionResult> StudentLabPage(string id)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            _logger.LogInformation($"User Email: {userEmail}"); 

            if (!string.IsNullOrEmpty(userEmail))
            {
                var student = await _studentService.FindByEmailAsync(userEmail);

                if (student != null)
                {

                    var labProgress = student.LabProgress.FirstOrDefault(lp => lp.LabId == id);

                    if (labProgress != null)
                    {
                        ViewBag.LabId = id;
                        ViewBag.LabComplete = labProgress.IsComplete;
                        ViewBag.TutorialComplete = labProgress.TutorialComplete;
                    }
                }
            }
            return View();
        }

        // Redirect to lab tutorial page
        [HttpGet("LabTutorial/{id}")]
        public async Task<IActionResult> LabTutorial(string id)
        {

            Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.Add("Pragma", "no-cache");
            Response.Headers.Add("Expires", "0");

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            _logger.LogInformation($"User Email: {userEmail}");

            if (!string.IsNullOrEmpty(userEmail))
            {
                var student = await _studentService.FindByEmailAsync(userEmail);

                if (student != null)
                {
                    var labProgress = student.LabProgress.FirstOrDefault(lp => lp.LabId == id);

                    if (labProgress != null && !labProgress.TutorialComplete) 
                    {
                        ViewBag.LabId = id;
                        return View();
                    }
                }
            }
            return RedirectToAction("StudentLabPage", new { id });
        }

        // Lab submission logic
        [HttpPost("CompleteTutorial/{labId}")]
        public async Task<IActionResult> CompleteTutorial(string labId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            _logger.LogInformation($"User Email: {userEmail}");

            if (!string.IsNullOrEmpty(userEmail))
            {
                var student = await _studentService.FindByEmailAsync(userEmail);

                if (student != null)
                {
                    var labProgress = student.LabProgress.FirstOrDefault(lp => lp.LabId == labId);
                    if (labProgress != null)
                    {
                        _logger.LogInformation($"Lab progress found for lab ID: {labId}");
                        labProgress.TutorialComplete = true;
                        await _studentService.UpdateStudentAsync(student);
                        _logger.LogInformation("Lab progress updated.");
                        return Ok();
                    }
                }
            }
            return NotFound();
        }

        // Redirect to lab simulation page
        [HttpGet("PlayLab/{id}")]
        public async Task<IActionResult> PlayLab(string id)
        {

            Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.Add("Pragma", "no-cache");
            Response.Headers.Add("Expires", "0");

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            _logger.LogInformation($"User Email: {userEmail}");

            if (!string.IsNullOrEmpty(userEmail))
            {
                var student = await _studentService.FindByEmailAsync(userEmail);

                if (student != null)
                {

                    var labProgress = student.LabProgress.FirstOrDefault(lp => lp.LabId == id);

                    if (labProgress != null && labProgress.TutorialComplete && !labProgress.IsComplete)
                    {
                        labProgress.Attempts++;
                        foreach (var checkpoint in labProgress.Checkpoints)
                        {
                            checkpoint.Timestamp = null;
                        }
                        labProgress.StartDate = DateTime.Now;
                        await _studentService.UpdateStudentAsync(student);
                        ViewBag.LabId = id;
                        return View();
                    }
                }
            }
            return RedirectToAction("StudentLabPage", new { id });
        }

        // Lab submission logic
        [HttpPost("CompleteLab/{labId}")]
        public async Task<IActionResult> CompleteLab(string labId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            _logger.LogInformation($"User Email: {userEmail}");

            if (!string.IsNullOrEmpty(userEmail))
            {
                var student = await _studentService.FindByEmailAsync(userEmail); 

                if (student != null)
                {
                    var labProgress = student.LabProgress.FirstOrDefault(lp => lp.LabId == labId);
                    if (labProgress != null)
                    {
                        labProgress.IsComplete = true;
                        labProgress.EndDate = DateTime.UtcNow;
                        await _studentService.UpdateStudentAsync(student);
                        return Ok();
                    }
                }
            }
            return NotFound();
        }


    }

}