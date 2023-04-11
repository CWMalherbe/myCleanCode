namespace Application.Interfaces;

/// <summary>
/// Interface representing an HTTP service that provides an instance of HttpClient.
/// </summary>
public interface IHttpService
{
    /// <summary>
    /// Gets an instance of HttpClient.
    /// </summary>
    HttpClient HttpClient { get; }
}