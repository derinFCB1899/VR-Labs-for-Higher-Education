using Microsoft.AspNetCore.Mvc;
using VR_Labs_for_Higher_Education.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet("StudentHomePage")]
        public IActionResult StudentHomePage()
        {
            return View();
        }

        [HttpGet("StudentProfilePage")]
        public IActionResult StudentProfilePage()
        {
            return View();
        }

        [HttpGet("StudentLabPage/{id}")]
        public IActionResult StudentLabPage(string id)
        {
            ViewData["LabId"] = id;
            return View();
        }

        [HttpGet("PlayLab/{id}")]
        public IActionResult PlayLab(string id)
        {
            // Path to the Unity WebGL index.html file
            var pathToUnityGame = "~/titrationLab.html";
            ViewBag.PathToUnityGame = pathToUnityGame;
            ViewBag.LabId = id; // Pass the 'id' parameter to the view
            return View("PlayLab");
        }

    }
}