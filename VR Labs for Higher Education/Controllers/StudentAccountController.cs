/*using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;
using VR_Labs_for_Higher_Education.Data;
using VR_Labs_for_Higher_Education.Models;

namespace VR_Labs_for_Higher_Education.Controllers
{
    public class StudentAccountController : Controller
    {
        private readonly MongoDbContext _mongoDbContext;

        // Constructor injection for MongoDbContext
        public StudentAccountController(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Return the view with validation errors
            }

            // Now _mongoDbContext should be available to use
            var student = await _mongoDbContext.Students.Find(s => s.Email == model.Username).FirstOrDefaultAsync();

            if (student != null && student.Password == model.Password) // Password should be hashed and checked securely
            {
                // Implement your logic for a successful login
                // E.g., creating an authentication cookie or token
                return RedirectToAction("Index", "Home"); // Redirect to the home page on successful login
            }

            // If the user is not found or password doesn't match, reload the login view with an error message
            ModelState.AddModelError(string.Empty, "Invalid username or password.");
            return View(model);
        }
    }
}*/