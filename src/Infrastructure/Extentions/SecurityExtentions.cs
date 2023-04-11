using Domain.Entities;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace Infrastructure.Extentions;

/// <summary>
/// Provides extension methods related to security functionality.
/// </summary>
public static class SecurityExtentions
{
    // Added this part for now as There is still quite a bit of work to do with this part.
    private const string THIRDPARTYURL = @"https://accounts.autocartruck.com/api/Auth0/applicationMetadata/";

    /// <summary>
    /// Extracts roles from an external source and adds them to the current user's claims.
    /// </summary>
    /// <param name="context">The context of the token being validated.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public static async Task ExtractRolesFromExternalSource(Microsoft.AspNetCore.Authentication.OpenIdConnect.TokenValidatedContext context)
    {
        string access_token = context.ProtocolMessage.AccessToken;
        string? userSub = context.SecurityToken.Payload["sub"].ToString();
        if (userSub == null)
        {
            // Should be useful to throw an exception here.
            return;
        }
        string userId = userSub.Split("|").Last();

        // TESTING
        //userId = "67045a04-d9b8-42dd-87a8-8023ab4ff32e";
        //

        AccountDetails? accountDetails = new AccountDetails
        {
            Roles = new string[] { }
        };


        //Unique, since we do not allow for catches, but could be added later
        try
        {
            using (HttpClient client = new HttpClient())
            {
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, THIRDPARTYURL + userId))
                {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

                    HttpResponseMessage? response = await client.SendAsync(requestMessage);
                    using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                    {
                        accountDetails = await JsonSerializer.DeserializeAsync<AccountDetails>(responseStream);
                    }
                }
            }
        }
        finally
        {
            List<Claim> claims = new List<Claim>();
            if (accountDetails != null && accountDetails.Roles != null)
            {
                foreach (var role in accountDetails.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                }
            }

            var identity = new ClaimsIdentity(claims);
            if (context.Principal != null)
            {
                context.Principal.AddIdentity(identity);
            }
        }
    }
}
