using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Presentation.Shared.Pages;

/// <summary>
/// Represents a Razor PageModel that handles errors and logs them using a specified ILogger instance.
/// </summary>
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    /// <summary>
    /// Gets or sets the request ID associated with the error.
    /// </summary>
    public string? RequestId { get; set; }
    /// <summary>
    /// Determines whether to show the request ID or not.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    private readonly ILogger<ErrorModel> _logger;

    /// <summary>
    /// Initializes a new instance of the ErrorModel class with the specified ILogger instance.
    /// </summary>
    /// <param name="logger">The ILogger instance to use for logging errors.</param>
    public ErrorModel(ILogger<ErrorModel> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles the HTTP GET request for the error page.
    /// </summary>
    public void OnGet()
    {
        // Sets the RequestId property to the current activity ID or generates a new one if none exists
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
}