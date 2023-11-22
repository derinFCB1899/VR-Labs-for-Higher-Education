/*using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using VR_Labs_for_Higher_Education.Data;

namespace VR_Labs_for_Higher_Education.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorsController : Controller
    {
        private readonly MongoDbContext _mongoDbContext;

        public InstructorsController(MongoDbContext mongoDbContext)
        {
            _mongoDbContext = mongoDbContext;
        }

        [HttpGet]
        public IActionResult GetInstructors()
        {
            var instructors = _mongoDbContext.Instructors.Find(instructor => true).ToList();
            return Ok(instructors);
        }

        [HttpGet("{id}")]
        public IActionResult GetInstructor(string id)
        {
            var instructor = _mongoDbContext.Instructors.Find(instructor => instructor.Id == id).FirstOrDefault();
            if (instructor == null)
            {
                return NotFound();
            }
            return Ok(instructor);
        }

        // Implement other CRUD operations for instructors if needed...
    }


}*/
