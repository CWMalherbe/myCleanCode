using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Presentation.Shared.Pages;

/// <summary>
/// Represents a page for displaying user information.
/// </summary>
public partial class UserPage
{
    /// <summary>
    /// The header to display on the page.
    /// </summary>
    private readonly string PageHeader = "User Info";

    /// <summary>
    /// The URL of the user's profile picture.
    /// </summary>
    private string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// The user's name.
    /// </summary>
    private string UserName { get; set; } = string.Empty;

    /// <summary>
    /// The current user's claims principal.
    /// </summary>
    private ClaimsPrincipal User { get; set; } = new ClaimsPrincipal();

    /// <summary>
    /// A parameter that receives the authentication state of the user from a parent component.
    /// </summary>
    [CascadingParameter]
    Task<AuthenticationState>? authenticationStateTask { get; set; }

    /// <summary>
    /// Initializes the component asynchronously when it is added to the component tree.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected async override Task OnInitializedAsync()
    {
        if (authenticationStateTask != null)
        {
            var user = (await authenticationStateTask).User;
            if (user != null)
            {
                User = user;
                Claim? tempClaim;
                tempClaim = user.Claims.FirstOrDefault(x => x.Type == "picture");
                if (tempClaim != null)
                {
                    ImageUrl = tempClaim.Value;
                }
                tempClaim = user.Claims.FirstOrDefault(x => x.Type == "name");
                if (tempClaim != null)
                {
                    UserName = tempClaim.Value;
                }
            }
        }
        await base.OnInitializedAsync();
    }
}