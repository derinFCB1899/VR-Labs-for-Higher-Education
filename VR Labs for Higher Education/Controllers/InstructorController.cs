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
    [Authorize(Roles = "Instructor")]
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

        // Display instructor home page
        [HttpGet("InstructorHomePage")]
        public IActionResult InstructorHomePage()
        {
            return View();
        }

        // Default redirect will be to the homepage
        [HttpGet]
        public IActionResult DefaultRedirect()
        {
            return RedirectToAction("InstructorHomePage", "Instructor");
        }

        // Display grading page for the instructor to grade students for a lab with its ID
        [HttpGet("InstructorGradePage/{id}")]
        public async Task<IActionResult> InstructorGradePage(string id)
        {
            ViewData["LabId"] = id;

            // Retrieve the list of students who completed the lab
            var studentsWithCompletedLabs = await _instructorService.GetStudentsCompletedLabAsync(id);

            // Pass the list of students to the view
            ViewData["StudentsWithCompletedLabs"] = studentsWithCompletedLabs;

            return View();
        }

        // Update a student's grade for a lab, given the studentId, labId and student grade
        [HttpPost("UpdateGrade")]
        public async Task<IActionResult> UpdateGrade()
        {
            // Retrieve the values posted from the form
            var studentId = Request.Form["studentId"];
            var labId = Request.Form["labId"];
            var newGrade = Request.Form["grade"];

            // Convert the newGrade to a double (you might want to add error handling)
            if (double.TryParse(newGrade, out var gradeValue))
            {
                // Call your service method to update the student's grade
                bool updateResult = await _instructorService.UpdateStudentGradeAsync(studentId, labId, gradeValue);

                if (updateResult)
                {
                    // Redirect back to the grade page with a success message
                    TempData["SuccessMessage"] = "Grade updated successfully.";
                }
                else
                {
                    // Redirect back with an error message
                    TempData["ErrorMessage"] = "Failed to update the grade.";
                }
            }
            else
            {
                // Handle the case where 'newGrade' couldn't be parsed as a double
                TempData["ErrorMessage"] = "Invalid grade format.";
            }

            // Redirect back to the InstructorGradePage with labId
            return RedirectToAction("InstructorGradePage", new { id = labId });
        }

    }
}