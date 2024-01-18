using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VR_Labs_for_Higher_Education.Models;
using VR_Labs_for_Higher_Education.Services;

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
            if (User.Identity.IsAuthenticated)
            {
                // Redirect to StudentHomePage if the user has the Student role
                if (User.IsInRole("Student"))
                {
                    return RedirectToAction("StudentHomePage", "Student");
                }
                // Redirect to InstructorHomePage if the user has the Student role
                else if (User.IsInRole("Instructor"))
                {
                    return RedirectToAction("InstructorHomePage", "Instructor");
                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}