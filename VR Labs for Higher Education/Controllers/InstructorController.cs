using Microsoft.AspNetCore.Mvc;
using VR_Labs_for_Higher_Education.Models;
using System.Threading.Tasks;
using VR_Labs_for_Higher_Education.Services;
using Microsoft.AspNetCore.Authorization;

namespace VR_Labs_for_Higher_Education.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InstructorController : Controller
    {
        private readonly InstructorService _instructorService;
        private readonly ILogger<InstructorController> _logger;

        public InstructorController(InstructorService instructorService, ILogger<InstructorController> logger)
        {
            _instructorService = instructorService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetInstructors()
        {
            var instructors = await _instructorService.GetInstructorsAsync();
            return Ok(instructors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstructor(string id)
        {
            var instructor = await _instructorService.GetInstructorAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return Ok(instructor);
        }

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

                    if (emailClaim != null && emailClaim.Value.EndsWith("@isikun.edu.tr"))
                    {
                        _logger.LogInformation($"Ensuring instructor record for {emailClaim.Value}");
                        var ınstructor = await _instructorService.EnsureInstructorRecord(emailClaim.Value, nameClaim?.Value);
                        _logger.LogInformation($"Redirecting to InstructorHomePage for {emailClaim.Value}");
                        return RedirectToAction("InstructorHomePage", "Instructor");
                    }
                    else
                    {
                        _logger.LogInformation($"Non-instructor login attempt for {nameClaim?.Value}");
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

        [HttpGet("InstructorHomePage")]
        [Authorize(Policy = "InstructorOnly")]
        public IActionResult InstructorHomePage()
        {
            ViewData["FullName"] = User.FindFirst(c => c.Type == "name")?.Value;
            return View();
        }
    }
}
