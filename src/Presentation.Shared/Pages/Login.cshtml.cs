using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Presentation.Shared.Pages;

/// <summary>
/// LoginModel class is a PageModel used for handling login requests.
/// </summary>
public class LoginModel : PageModel
{
    /// <summary>
    /// Asynchronously handles HTTP GET requests for login.
    /// </summary>
    /// <param name="redirectUri">The URL to redirect to after successful login.</param>
    public async Task OnGet(string redirectUri)
    {
        if (string.IsNullOrWhiteSpace(redirectUri))
        {
            redirectUri = Url.Content("~/");
        }

        // Redirect to the specified URL if the user is already authenticated.
        if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
        {
            Response.Redirect(redirectUri);
        }

        // Challenge the user to authenticate using OpenIdConnectDefaults.AuthenticationScheme.
        await HttpContext.ChallengeAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
        {
            RedirectUri = redirectUri
        });
    }
}