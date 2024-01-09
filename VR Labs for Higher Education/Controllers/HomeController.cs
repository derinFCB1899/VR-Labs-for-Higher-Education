using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VR_Labs_for_Higher_Education.Models;

namespace VR_Labs_for_Higher_Education.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            // Check if the user is authenticated
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to StudentHomePage if the user has the Student role
                if (User.IsInRole("Student"))
                {
                    return RedirectToAction("StudentHomePage", "Student");
                }
                // Redirect to InstructorHomePage if the user has the Instructor role
                else if (User.IsInRole("Instructor"))
                {
                    return RedirectToAction("InstructorHomePage", "Instructor");
                }
                // Optionally, handle other roles or default case
            }

            // For non-authenticated users, show the default index page
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult PlayGame()
        {
            return View();
        }

    }
}