using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using VR_Labs_for_Higher_Education.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

public class AccountController : Controller
{
    private readonly StudentService _studentService; // Assuming you have a service layer
    private readonly InstructorService _instructorService; // Assuming you have a service layer

    public AccountController(StudentService studentService, InstructorService instructorService)
    {
        _studentService = studentService;
        _instructorService = instructorService;
    }

    public IActionResult Login(string returnUrl = "/")
    {
        return Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, OpenIdConnectDefaults.AuthenticationScheme);
    }


    public async Task<IActionResult> Logout()
    {
        // Clear the existing external cookie
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // Redirect to Microsoft Identity platform's logout endpoint
        var logoutUrl = "https://login.microsoftonline.com/98ae62f4-d739-4876-84f5-0e01d3390f9d/oauth2/v2.0/logout?post_logout_redirect_uri=https://localhost:7091/signin-oidc";
        return new SignOutResult(new[] { CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme },
                                 new AuthenticationProperties { RedirectUri = logoutUrl });
    }

}
