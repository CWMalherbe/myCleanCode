using System.Data;
using Application.Interfaces;
using Application.Models;
using Domain.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Security;
/// <summary>
/// Security service implementation of IdentityService. 
/// Currently not implemented.
/// </summary>
public class IdentityService : IIdentityService
{
    private readonly UserManager<BaseUser> _userManager;
    private readonly IUserClaimsPrincipalFactory<BaseUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;

    /// <summary>
    /// Manages users
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="userClaimsPrincipalFactory"></param>
    /// <param name="authorizationService"></param>
    public IdentityService(UserManager<BaseUser> userManager, IUserClaimsPrincipalFactory<BaseUser> userClaimsPrincipalFactory, IAuthorizationService authorizationService)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="policyName"></param>
    /// <returns></returns>
    public async Task<bool> AuthorizeAsync(int userId, string policyName)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);
        if (user == null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<(Result Result, int UserId)> CreateUserAsyncWithPassword(string userName, string password)
    {
        var user = new BaseUser
        {
            UserName = userName,
            Email = userName,
        };

        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            return (Result.Ok(), user.Id);
        }
        return (Result.Error(result.Errors.Select(e => e.Description)), 0);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="identifier"></param>
    /// <returns></returns>
    public async Task<(Result Result, int UserId)> CreateUserAsyncWithIdentifier(string userName, string identifier)
    {
        var user = new BaseUser
        {
            UserName = userName,
            Email = userName,
            Identifier = identifier
        };

        var result = await _userManager.CreateAsync(user);
        if (result.Succeeded)
        {
            return (Result.Ok(), user.Id);
        }
        return (Result.Error(result.Errors.Select(e => e.Description)), 0);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<Result> DeleteUserAsync(int userId)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null ? await DeleteUserAsync(user) : Result.Ok();
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<Result> DeleteUserAsync(BaseUser user)
    {
        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return Result.Ok();
        }
        return Result.Error(result.Errors.Select(e => e.Description));
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<string> GetUserNameAsync(int userId)
    {
        BaseUser? user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
        if (user != null && user.UserName != null)
        {
            return user.UserName;
        }
        return "Anon";
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    public async Task<bool> IsInRoleAsync(int userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);
        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<(Result Result, int UserId)> CreateUserAsync(string userName)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="identifier"></param>
    /// <returns></returns>
    public async Task<int> GetUserIdByIdentification(string identifier)
    {
        var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Identifier == identifier);
        return user != null ? user.Id : 0;
    }
}
