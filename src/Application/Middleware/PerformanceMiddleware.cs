using System.Diagnostics;
using Application.Interfaces;
using MediatR;
using ILogger = Application.Interfaces.ILogger;

namespace Application.Middleware;
/// <summary>
/// Great for logging performance details. Can be used to see when response times exceed what we expect.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class PerformanceMiddleware<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : MediatR.IRequest<TResponse>
{
    private readonly ILogger _logger;
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;
    //private const int REQUEST_MAX_TICKS = 5000000; // Remember a tick is 100ns or 0.1µs or 0.00001ms
    // FOR TESTING, ADDED ALL CALLS
    private const int REQUEST_MAX_TICKS = 0; // Remember a tick is 100ns or 0.1µs or 0.00001ms

    /// <summary>
    /// Great for logging performance details. Can be used to see when response times exceed what we expect.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="currentUserService"></param>
    /// <param name="identityService"></param>
    public PerformanceMiddleware(
        ILogger logger,
        ICurrentUserService currentUserService,
        IIdentityService identityService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
        _identityService = identityService;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        long elapsedTicks = Stopwatch.GetTimestamp();
        // If we want to see the performance on exceptions as well, we could add a try finally here
        var response = await next();

        elapsedTicks = Stopwatch.GetTimestamp() - elapsedTicks;

        if (elapsedTicks > REQUEST_MAX_TICKS)
        {

            var requestName = typeof(TRequest).Name;
            string userName = _currentUserService.UserName;
           _logger.Warn("TheTemplate Long Running Request: {requestName} ({elapsedMilliseconds} milliseconds) {userName} {request}", "PerformanceBehaviour.Handle",
                requestName, elapsedTicks / 10000, userName, request);
        }

        return response;
    }
}