namespace Application.Interfaces;

/// <summary>
/// Defines the interface for an access token storage mechanism.
/// </summary>
public interface IAccessTokenStorage
{
    /// <summary>
    /// Gets the access token asynchronously.
    /// </summary>
    /// <returns>The access token string.</returns>
    Task<string> GetTokenAsync();

    /// <summary>
    /// Sets the access token asynchronously.
    /// </summary>
    /// <param name="token">The access token string to set.</param>
    Task SetTokenAsync(string token);
}