using Serilog;
using Serilog.Formatting.Json;

namespace Infrastructure.Factory;

/// <summary>
/// Factory used to create logger. 
/// I prefer to use Serilog. Quite fast. Modern. 
/// It also allows us to use most log viewing tools like Grafana and Prometheus.
/// </summary>
public static class LoggingFactory
{
    /// <summary>
    /// Generates a SeriLogger
    /// </summary>
    /// <returns></returns>
    public static ILogger GetSeriLogger()
    {
        LoggerConfiguration configuration = new LoggerConfiguration()
            .WriteTo.File(new JsonFormatter(renderMessage: true), "logs.json", rollingInterval: RollingInterval.Day)
            .WriteTo.Console(theme: Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme.Literate)
            .MinimumLevel.Is(Serilog.Events.LogEventLevel.Debug);
        return configuration.CreateLogger();
    }
}
