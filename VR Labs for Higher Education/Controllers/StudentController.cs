using Microsoft.AspNetCore.Mvc;
using VR_Labs_for_Higher_Education.Models;
using System.Threading.Tasks;
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
                        LabProgresses = labProgresses // Pass the LabProgress data to the view model
                    };

                    return View(viewModel);
                }
            }
            // Handle the case where the student or email is not found
            return NotFound();
        }

        // Redirect to lab preview page
        [HttpGet("StudentLabPage/{id}")]
        public async Task<IActionResult> StudentLabPage(string id)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email); // Using email to find the user
            _logger.LogInformation($"User Email: {userEmail}"); // Logging the user email

            if (!string.IsNullOrEmpty(userEmail))
            {
                var student = await _studentService.FindByEmailAsync(userEmail); // Finding the student by email

                if (student != null)
                {
                    // Retrieve the lab progress for the specific lab (assuming LabProgress has labComplete and tutorialComplete fields)
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
            var userEmail = User.FindFirstValue(ClaimTypes.Email); // Using email to find the user
            _logger.LogInformation($"User Email: {userEmail}"); // Logging the user email

            if (!string.IsNullOrEmpty(userEmail))
            {
                var student = await _studentService.FindByEmailAsync(userEmail); // Finding the student by email

                if (student != null)
                {
                    // Retrieve the lab progress for the specific lab (assuming LabProgress has TutorialComplete field)
                    var labProgress = student.LabProgress.FirstOrDefault(lp => lp.LabId == id);

                    if (labProgress != null && !labProgress.TutorialComplete) // Check if the tutorial is not marked as complete
                    {
                        // Page was accessed, the student has successfully accessed the tutorial.
                        labProgress.TutorialComplete = true;
                        await _studentService.UpdateStudentAsync(student);
                        _logger.LogInformation("Updating the tutorial status.");
                        ViewData["LabId"] = id;
                        return View();
                    }
                }
            }
            // If the tutorial is already completed or user information is not found, you can handle it here (e.g., redirect to another page or show a message)
            _logger.LogInformation("Tutorial has already been completed or user information not found.");
            return RedirectToAction("StudentLabPage", new { id }); // Redirect to StudentLabPage or handle it accordingly
        }

        // Redirect to lab simulation page
        [HttpGet("PlayLab/{id}")]
        public async Task<IActionResult> PlayLab(string id)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email); // Using email to find the user
            _logger.LogInformation($"User Email: {userEmail}"); // Logging the user email

            if (!string.IsNullOrEmpty(userEmail))
            {
                var student = await _studentService.FindByEmailAsync(userEmail); // Finding the student by email

                if (student != null)
                {
                    // Retrieve the lab progress for the specific lab (assuming LabProgress has IsComplete field)
                    var labProgress = student.LabProgress.FirstOrDefault(lp => lp.LabId == id);

                    if (labProgress != null && labProgress.TutorialComplete && !labProgress.IsComplete)
                    {
                        // Access the lab with the Unity file.
                        var pathToUnityGame = "~/titrationLab.html";
                        ViewBag.PathToUnityGame = pathToUnityGame;
                        ViewBag.LabId = id;
                        return View("PlayLab");
                    }
                }
            }
            // The user has already completed the lab or unauthorized, therefore return to StudentLabPage
            _logger.LogInformation("Lab has already been completed or user information not found.");
            return RedirectToAction("StudentLabPage", new { id });
        }

        // Lab submission logic
        [HttpPost("CompleteLab/{labId}")]
        public async Task<IActionResult> CompleteLab(string labId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email); // Using email to find the user
            _logger.LogInformation($"User Email: {userEmail}"); // Logging the user email

            if (!string.IsNullOrEmpty(userEmail))
            {
                var student = await _studentService.FindByEmailAsync(userEmail); // Finding the student by email

                if (student != null)
                {
                    var labProgress = student.LabProgress.FirstOrDefault(lp => lp.LabId == labId);
                    if (labProgress != null)
                    {
                        _logger.LogInformation($"Lab progress found for lab ID: {labId}"); // Logging the lab ID
                        labProgress.IsComplete = true;
                        labProgress.EndDate = DateTime.UtcNow;
                        await _studentService.UpdateStudentAsync(student);
                        _logger.LogInformation("Lab progress updated."); // Logging the update
                        return Ok();
                    }
                }
            }

            _logger.LogInformation("Lab progress or user email not found."); // Logging when lab progress or email is not found
            return NotFound();
        }

    }

}