using System.Security.Claims;
using Application.Interfaces;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components.Authorization;

namespace Presentation.WebServer.Services;

/// <summary>
/// <inheritdoc/>
/// </summary>
public class AccessBearerService : IAccessBearer
{
    /// <summary>
    /// Adds a bearer token to the specified HttpClient instance based on the given AuthenticationState task.
    /// </summary>
    /// <param name="authenticationStateTask">A task representing the current authentication state.</param>
    /// <param name="httpClient">The HttpClient instance to authenticate.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task AddBearerToken(Task<AuthenticationState>? authenticationStateTask, HttpClient httpClient)
    {
        if (authenticationStateTask != null)
        {
            // Get the authenticated user from the authentication state task
            var user = (await authenticationStateTask).User;

            // Find the access_token claim from the user's claims collection
            Claim? tempClaim = user.Claims.FirstOrDefault(x => x.Type == "access_token");

            // If the access_token claim exists, set the bearer token for the HttpClient instance
            if (tempClaim != null)
            {
                httpClient.SetBearerToken(tempClaim.Value);
                return;
            }
        }
        // Otherwise, set the bearer token to an empty string
        httpClient.SetBearerToken(string.Empty);
    }
}
