using Application.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Presentation.WebServer.Services;


/// <summary>
/// Provides a configured HttpClient instance that can be used to make HTTP requests.
/// </summary>
public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpService"/> class.
    /// </summary>
    /// <param name="httpClient">The HttpClient instance to use for making HTTP requests.</param>
    /// <param name="navigationManager">The NavigationManager instance to use for getting the base URI.</param>
    public HttpService(HttpClient httpClient, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
    }

    /// <summary>
    /// Gets a configured HttpClient instance that can be used to make HTTP requests.
    /// If the BaseAddress property of the HttpClient has not been set, it will be set to the base URI of the application.
    /// </summary>
    public HttpClient HttpClient
    {
        get
        {
            if (_httpClient.BaseAddress == null)
            {
                _httpClient.BaseAddress = new Uri(_navigationManager.BaseUri);
            }
            return _httpClient;
        }
    }
}