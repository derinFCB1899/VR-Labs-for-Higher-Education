using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using VR_Labs_for_Higher_Education.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using VR_Labs_for_Higher_Education.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;

public class AccountController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly StudentService _studentService; // Assuming you have a service layer
    private readonly InstructorService _instructorService; // Assuming you have a service layer
    private readonly ILogger<AccountController> _logger;

    public AccountController(IConfiguration configuration, StudentService studentService, InstructorService instructorService, ILogger<AccountController> logger)
    {
        _configuration = configuration;
        _studentService = studentService;
        _instructorService = instructorService;
        _logger = logger;
    }

    // Login Form Display
    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            // Student redirect
            if (User.IsInRole("Student"))
            {
                return RedirectToAction("StudentHomePage", "Student");
            }
            //Instructor redirect
            else if (User.IsInRole("Instructor"))
            {
                return RedirectToAction("InstructorHomePage", "Instructor");
            }
        }
        // If the user is not authenticated, show the login page
        return View();
    }


    // Login action (Information check, verification, redirect etc.)
    [HttpPost]
    public async Task<IActionResult> Login(LoginDTO model)
    {
        _logger.LogInformation("Login attempt for {Email}", model.Email);

        if (!ModelState.IsValid)
        {
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    _logger.LogWarning("Error in {Field}: {ErrorMessage}", state.Key, error.ErrorMessage);
                }
            }
            return View(model);
        }

        object user = null;
        string role = "";
        string userName = "";

        // Determine user role based on email domain
        if (model.Email.EndsWith("@isik.edu.tr"))
        {
            user = await _studentService.FindByEmailAsync(model.Email);
            if (user != null && _studentService.VerifyPassword(user as Student, model.Password))
            {
                role = "Student";
                userName = (user as Student).Name;
            }
        }
        else if (model.Email.EndsWith("@isikun.edu.tr"))
        {
            user = await _instructorService.FindByEmailAsync(model.Email);
            if (user != null && _instructorService.VerifyPassword(user as Instructor, model.Password))
            {
                role = "Instructor";
                userName = (user as Instructor).Name;
            }
        }

        if (string.IsNullOrEmpty(role))
        {
            _logger.LogWarning("Login failed for {Email}", model.Email);

            // Display an error message to the user
            ViewBag.ErrorMessage = "Wrong username or password, try again.";

            return View(model);
        }

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Email, model.Email),
        new Claim(ClaimTypes.Role, role),
        new Claim("name", userName)
    };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        _logger.LogInformation("User signed in, redirecting to {Role} home page", role);

        // Redirect to the appropriate home page based on the user's role
        if (role == "Student")
        {
            return RedirectToAction("StudentHomePage", "Student");
        }
        else if (role == "Instructor")
        {
            return RedirectToAction("InstructorHomePage", "Instructor");
        }

        // Default redirect if role is not recognized
        return RedirectToAction("Index", "Home");
    }

    // Logout action and redirect
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        _logger.LogInformation("User logged out successfully");

        return RedirectToAction("Index", "Home");
    }
}