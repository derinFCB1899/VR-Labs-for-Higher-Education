using Microsoft.AspNetCore.Mvc;
using VR_Labs_for_Higher_Education.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace VR_Labs_for_Higher_Education.Controllers
{
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


        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var students = await _studentService.GetStudentsAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(string id)
        {
            _logger.LogInformation($"Attempting to retrieve student with ID: {id}");
            var student = await _studentService.GetStudentAsync(id);
            if (student == null)
            {
                _logger.LogWarning($"Student not found with ID: {id}");
                return NotFound();
            }
            _logger.LogInformation($"Student retrieved with ID: {id}");
            return Ok(student);
        }

        // Additional actions can be added here for create, update, delete operations
        [Authorize(Policy = "AllowedUsers")]
        [HttpGet("signin-callback")]
        public async Task<IActionResult> SignInCallback()
        {
            _logger.LogInformation("SignInCallback accessed");
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    var emailClaim = User.FindFirst(c => c.Type == "preferred_username");
                    var nameClaim = User.FindFirst(c => c.Type == "name");
                    _logger.LogInformation($"Email claim: {emailClaim?.Value}");
                    _logger.LogInformation($"Name claim: {nameClaim?.Value}");

                    if (emailClaim != null && emailClaim.Value.EndsWith("@isik.edu.tr"))
                    {
                        _logger.LogInformation($"Ensuring student record for {emailClaim.Value}");
                        var student = await _studentService.EnsureStudentRecord(emailClaim.Value, nameClaim?.Value);
                        _logger.LogInformation($"Redirecting to StudentHomePage for {emailClaim.Value}");
                        return RedirectToAction("StudentHomePage", "Student");
                    }
                    else
                    {
                        _logger.LogInformation($"Non-student login attempt for {nameClaim?.Value}");
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error in SignInCallback: {ex.Message}");
                    return RedirectToAction("SignInFailure");
                }
            }
            else
            {
                _logger.LogInformation("User is not authenticated.");
                return RedirectToAction("SignInFailure");
            }
        }

        [HttpGet("StudentHomePage")]
        public IActionResult StudentHomePage()
        {
            ViewData["FullName"] = User.FindFirst(c => c.Type == "name")?.Value;
            return View();
        }

    }

}