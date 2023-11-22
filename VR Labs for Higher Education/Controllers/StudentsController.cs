using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using VR_Labs_for_Higher_Education.Data;
using VR_Labs_for_Higher_Education.Models;

namespace VR_Labs_for_Higher_Education.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : Controller
    {
        private readonly MongoDbContext _mongoDbContext;

        public StudentsController(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = _mongoDbContext.Students.Find(student => true).ToList();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(string id)
        {
            var student = _mongoDbContext.Students.Find(student => student.Id == id).FirstOrDefault();
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // In a real application, you should hash the incoming password and compare it to the stored hash
            var student = _mongoDbContext.Students.Find(s => s.Email == loginViewModel.Username && s.Password == loginViewModel.Password).FirstOrDefault();

            if (student == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            // If the student is found and the password matches, proceed to create a session or token.
            // For demonstration, we're just returning a success message.

            // TODO: Create an authentication ticket, session, or JWT token
            return Ok("Student logged in successfully.");
        }

    }

}
