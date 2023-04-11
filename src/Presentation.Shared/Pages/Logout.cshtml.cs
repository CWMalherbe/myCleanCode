using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Shared.Pages;

/// <summary>
/// LogoutModel class is a PageModel used for handling logout requests.
/// </summary>
public class LogoutModel : PageModel
{
    /// <summary>
    /// Asynchronously handles HTTP GET requests for logout.
    /// </summary>
    /// <returns>An IActionResult object representing the result of the logout operation.</returns>
    public async Task<IActionResult> OnGetAsync()
    {
        // Sign out the user from both the cookie authentication scheme and the OpenID Connect authentication scheme.
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        // Redirect the user to the home page after logging out.
        return LocalRedirect(Url.Content("~/"));
    }
}