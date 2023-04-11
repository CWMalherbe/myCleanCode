using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;

namespace Presentation.Shared.Components;

/// <summary>
/// This class represents a component that displays user information, including a user image and name.
/// </summary>
public partial class UserControlComponent
{
    /// <summary>
    /// The URL of the image associated with the user.
    /// </summary>
    private string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// The name of the user.
    /// </summary>
    private string UserName { get; set; } = string.Empty;

    /// <summary>
    /// The task that retrieves the user authentication state.
    /// </summary>
    [CascadingParameter]
    public Task<AuthenticationState>? authenticationStateTask { get; set; }

    /// <summary>
    /// Overrides the OnInitializedAsync method to retrieve the user image and name from the authentication state.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected async override Task OnInitializedAsync()
    {
        if (authenticationStateTask != null)
        {
            var user = (await authenticationStateTask).User;
            if (user != null)
            {
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
            await base.OnInitializedAsync();
        }
    }
}
