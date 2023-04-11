using System.Security.Claims;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.CurrentUserService;
/// <summary>
/// Get userId, currently not implemented.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IJWTAuthentication _jwtAuthentication;

    /// <summary>
    /// Instantiates get user interface
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    /// <param name="jwtAuthentication"></param>
    public CurrentUserService(IHttpContextAccessor httpContextAccessor, IJWTAuthentication jwtAuthentication)
    {
        _httpContextAccessor = httpContextAccessor;
        _jwtAuthentication = jwtAuthentication;
    }

    //OLD
    /// <summary>
    /// Integer value that represents the current user.
    /// </summary>
    [Obsolete("Method1 is deprecated, use method Username instead")]
    public int UserId
    {
        get
        {
            //VERY UNSURE ABOUT THIS PART BUT WILL FIGURE IT OUT IN THE END
            string? tempUser = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrWhiteSpace(tempUser))
            {
                return int.Parse(tempUser);
            }
            //VERY UNSURE ABOUT THIS PART AS WELL, BUT CLEVER GUYS CAN ASSIST
            else if (_httpContextAccessor.HttpContext != null &&
                _httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                return _jwtAuthentication.ValidateToken(
                        _httpContextAccessor.HttpContext.Request.Headers["Authorization"]
                        .ToString()
                        .Split(" ")
                        .Last()
                        );
            }
            return 0;
        }
    }

    /// <summary>
    /// String value that represents the current user.
    /// </summary>
    public string UserName
    {
        get
        {
            string? tempUser = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(tempUser))
            {
                return tempUser;
            }
            //VERY UNSURE ABOUT THIS PART AS WELL, BUT CLEVER GUYS CAN ASSIST
            //REMOVING THIS FOR NOW, SINCE IT WILL TRY TO COLLECT DATA WITHOUT AUTHORIZATION TAG
            /*
            else if (_httpContextAccessor.HttpContext != null &&
                _httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                return _jwtAuthentication.ValidateUsernameFromToken(
                        _httpContextAccessor.HttpContext.Request.Headers["Authorization"]
                        );
            }
            */
            return "Unknown User";
        }
    }
}
