using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Presentation.Shared.Components;

public partial class UserPopoverComponent
{
    private string ImageUrl { get; set; } = string.Empty;
    private string UserName { get; set; } = string.Empty;
    private bool PopoverOpenToggle { get; set; } = false;
    [CascadingParameter] Task<AuthenticationState>? authenticationStateTask { get; set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
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

    private void ActionLogout()
    {
        NavigationManager.NavigateTo("/logout", true);
    }

    private void TogglePopoverOpen()
    {
        PopoverOpenToggle = !PopoverOpenToggle;
    }
}
