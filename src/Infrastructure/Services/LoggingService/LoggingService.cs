using Microsoft.Extensions.Configuration;
using Serilog;

namespace Infrastructure.Services.LoggingService;
/// <summary>
/// Basics of the logging service. 
/// There are some nauses when creating the logs, but 
/// sure you will figure it out.
/// </summary>
public class LoggingService : Application.Interfaces.ILogger
{
    private readonly ILogger _logger;
    private readonly string _applicationName;
    /// <summary>
    /// Logger Service constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="configuration"></param>
    public LoggingService(ILogger logger, IConfiguration configuration)
    {
        _logger = logger;
        string? tempApplicationName = configuration.GetValue<string>("ApplicationName");
        _applicationName = string.IsNullOrEmpty(tempApplicationName) ? "UnknownApplication" : tempApplicationName;
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="messageTemplate"></param>
    /// <param name="callingfunction"></param>
    /// <param name="objects"></param>
    public void Debug(string messageTemplate, string callingfunction, params object[] objects)
    {
        _logger.Debug($"[{_applicationName}.{callingfunction}] {messageTemplate}", objects);
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="messageTemplate"></param>
    /// <param name="callingfunction"></param>
    /// <param name="objects"></param>
    public void Error(string messageTemplate, string callingfunction, params object[] objects)
    {
        _logger.Debug($"[{_applicationName}.{callingfunction}] {messageTemplate}", objects);
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="messageTemplate"></param>
    /// <param name="callingfunction"></param>
    /// <param name="objects"></param>
    public void Fatal(string messageTemplate, string callingfunction, params object[] objects)
    {
        _logger.Debug($"[{_applicationName}.{callingfunction}] {messageTemplate}", objects);
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="messageTemplate"></param>
    /// <param name="callingfunction"></param>
    /// <param name="objects"></param>
    public void Info(string messageTemplate, string callingfunction, params object[] objects)
    {
        _logger.Debug($"[{_applicationName}.{callingfunction}] {messageTemplate}", objects);
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="messageTemplate"></param>
    /// <param name="callingfunction"></param>
    /// <param name="objects"></param>
    public void Warn(string messageTemplate, string callingfunction, params object[] objects)
    {
        _logger.Debug($"[{_applicationName}.{callingfunction}] {messageTemplate}", objects);
    }
}
