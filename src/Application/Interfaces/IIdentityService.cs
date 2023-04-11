using Application.Models;

namespace Application.Interfaces;
/// <summary>
/// Good start, but not implemented
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// Gets the Specific username and returns as userId
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<string> GetUserNameAsync(int userId);

    /// <summary>
    /// Gets the Specific identifier and returns as userId
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns></returns>
    Task<int> GetUserIdByIdentification(string identifier);

    /// <summary>
    /// Searches roles
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    Task<bool> IsInRoleAsync(int userId, string role);

    /// <summary>
    /// Authorize action
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="policyName"></param>
    /// <returns></returns>
    Task<bool> AuthorizeAsync(int userId, string policyName);

    /// <summary>
    /// Action for creating a user. 
    /// Debates on wether or not this section needs to be added to all, 
    /// or just to the authentication app.
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<(Result Result, int UserId)> CreateUserAsyncWithPassword(string userName, string password);

    /// <summary>
    /// Action for creating a user. 
    /// Debates on wether or not this section needs to be added to all, 
    /// or just to the authentication app.
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="identifier"></param>
    /// <returns></returns>
    Task<(Result Result, int UserId)> CreateUserAsyncWithIdentifier(string userName, string identifier);

    /// <summary>
    /// Deletes the user.
    /// Debates on wether or not this section needs to be added to all, 
    /// or just to the authentication app.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<Result> DeleteUserAsync(int userId);
}
