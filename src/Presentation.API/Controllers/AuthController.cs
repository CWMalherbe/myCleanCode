using Application.Security.Commands.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Controllers;

/// <summary>
/// <inheritdoc/>
/// </summary>
public class AuthController : ApiControllerBase
{
    /// <summary>
    /// Authorizes the user through api methodology
    /// </summary>
    /// <param name="command"></param>
    /// <returns>A JWT that can be used for bearer auth activities</returns>
    [HttpPost]
    public async Task<ActionResult<string>> AuthenticateUser(AuthenticateUserCommand command)
    {
        return await Mediator.Send(command);
    }
}
