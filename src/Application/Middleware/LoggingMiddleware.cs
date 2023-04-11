using Application.Interfaces;
using MediatR.Pipeline;
using ILogger = Application.Interfaces.ILogger;

namespace Application.Middleware;
/// <summary>
/// Sends out logs before the action is performed. Useful for debugging.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
public class LoggingMiddleware<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Sends out logs before the action is performed. Useful for debugging.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="currentUserService"></param>
    /// <param name="identityService"></param>
    public LoggingMiddleware(ILogger logger, ICurrentUserService currentUserService, IIdentityService identityService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
        _identityService = identityService;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        string userName = _currentUserService.UserName;
        _logger.Info("Request: {requestName} {userName} {request}", "LoggingBehaviour.Process",
            requestName, userName, request);
        return Task.CompletedTask;
    }
}