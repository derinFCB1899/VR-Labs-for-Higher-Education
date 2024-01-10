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

        [HttpGet("StudentHomePage")]
        [Authorize(Roles = "Student")]
        public IActionResult StudentHomePage()
        {
            // You can include logic here to fetch and display student-specific information
            return View();
        }

        [HttpGet("StudentLabPage/{id}")]
        [Authorize(Roles = "Student")]
        public IActionResult StudentLabPage(string id)
        {
            // Temporarily using ViewData to pass the lab ID to the view
            // Ideally, you would fetch the lab details from the database using a lab service
            ViewData["LabId"] = id;

            // You could also use ViewBag
            // ViewBag.LabId = id;

            return View();
        }

        // Add to your StudentController

        [HttpGet("PlayLab")]
        //[Authorize(Roles = "Student")]
        public IActionResult PlayLab()
        {
            // Path to the Unity WebGL index.html file
            var pathToUnityGame = "~/titrationLab.html";
            ViewBag.PathToUnityGame = pathToUnityGame;
            return View("PlayLab");
        }

        [HttpGet("StudentProfilePage")]
        [Authorize(Roles = "Student")]
        public IActionResult StudentProfilePage()
        {
            return View();
        }

    }
}