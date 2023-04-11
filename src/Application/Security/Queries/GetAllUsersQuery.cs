using Domain.Bases;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Security.Queries;
/// <summary>
/// <inheritdoc/>
/// </summary>
public record GetAllUsersQuery : IRequest<IList<BaseUser>>
{
}

/// <summary>
/// <inheritdoc/>
/// </summary>
public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IList<BaseUser>>
{
    private readonly UserManager<BaseUser> _userManager;

    /// <summary>
    /// Silly function to allow for getting all the users
    /// </summary>
    /// <param name="userManager"></param>
    public GetAllUsersQueryHandler(UserManager<BaseUser> userManager)
    {
        _userManager = userManager;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IList<BaseUser>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await _userManager.Users.ToListAsync();
    }
}