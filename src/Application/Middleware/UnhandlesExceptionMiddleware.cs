using MediatR;
using ILogger = Application.Interfaces.ILogger;

namespace Application.Middleware;
/// <summary>
/// Middleware that puts a try catch around an entire event.
/// This is useful to catch exceptions but still throw them.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class UnhandledExceptionMiddleware<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : MediatR.IRequest<TResponse>
{
    private readonly ILogger _logger;

    /// <summary>
    /// Middleware that puts a try catch around an entire event.
    /// </summary>
    /// <param name="logger"></param>
    public UnhandledExceptionMiddleware(ILogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (System.Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            _logger.Error("Request: Unhandled Exception for Request {requestName} {request} {ex}", requestName, request, ex);

            throw;
        }
    }
}
