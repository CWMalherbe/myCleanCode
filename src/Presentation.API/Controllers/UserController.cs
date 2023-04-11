using System.Collections.Immutable;
using Application.Models;
using Application.Security.Commands.Authorization;
using Application.Security.Queries;
using Domain.Bases;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Controllers;

/// <summary>
/// Silly Controller for creating and reading users
/// </summary>
public class UserController : ApiControllerBase
{
    /// <summary>
    /// Creates a user
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<(Result, int)>> CreateUser(GenerateUserCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Reads all users
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IList<BaseUser>>> GetUsers()
    {
        return (await Mediator.Send(new GetAllUsersQuery())).ToImmutableList();
    }
}
