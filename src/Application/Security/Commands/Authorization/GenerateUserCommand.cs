using Application.Interfaces;
using Application.Models;
using MediatR;

namespace Application.Security.Commands.Authorization;

/// <summary>
/// General command to create a user
/// </summary>
public record GenerateUserCommand : IRequest<(Result, int)>
{
    /// <summary>
    /// Common username used for both email and username
    /// </summary>
    public string? username { get; init; }
    /// <summary>
    /// Identifier specific to Auth0
    /// </summary>
    public string? identifier { get; init; }
}
/// <summary>
/// <inheritdoc/>
/// </summary>
public class GenerateUserCommandHandler : IRequestHandler<GenerateUserCommand, (Result, int)>
{
    private readonly IIdentityService _identityService;

    /// <summary>
    /// General command to create a user
    /// </summary>
    /// <param name="identityService"></param>
    public GenerateUserCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<(Result, int)> Handle(GenerateUserCommand request, CancellationToken cancellationToken)
    {
        if (request.username != null && request.identifier != null)
        {
            return await _identityService.CreateUserAsyncWithIdentifier(request.username, request.identifier);
        }
        return (Result.Error(new List<string> { "username or password incorrect" }), 0);
    }
}
