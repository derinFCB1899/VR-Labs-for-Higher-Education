using Microsoft.AspNetCore.Mvc;
using VR_Labs_for_Higher_Education.Models;
using System.Threading.Tasks;
using VR_Labs_for_Higher_Education.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

        [HttpGet("InstructorHomePage")]
        [Authorize(Roles = "Instructor")]
        public IActionResult InstructorHomePage()
        {
            return View();
        }

        [HttpGet("InstructorProfilePage")]
        [Authorize(Roles = "Instructor")]
        public IActionResult InstructorProfilePage()
        {
            return View();
        }

    }
}