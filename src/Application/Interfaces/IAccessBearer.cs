using Microsoft.AspNetCore.Components.Authorization;

namespace Application.Interfaces;

/// <summary>
/// Provides extension methods to augment authentication
/// </summary>
public interface IAccessBearer
{
    /// <summary>
    /// Used to access and add the token to the httpclient
    /// </summary>
    /// <param name="authenticationStateTask"></param>
    /// <param name="httpClient"></param>
    /// <returns></returns>
    Task AddBearerToken(Task<AuthenticationState>? authenticationStateTask, HttpClient httpClient);
}
