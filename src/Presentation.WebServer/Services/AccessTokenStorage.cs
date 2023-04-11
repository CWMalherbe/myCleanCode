using Application.Interfaces;
using Microsoft.JSInterop;

namespace Presentation.WebServer.Services;

/// <summary>
/// Provides functionality to get and set the access token in local storage of the browser.
/// </summary>
public class AccessTokenStorage : IAccessTokenStorage
{
    private readonly IJSRuntime _jsRuntime;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessTokenStorage"/> class with the specified JS runtime.
    /// </summary>
    /// <param name="jsRuntime">The JS runtime.</param>
    public AccessTokenStorage(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    /// <summary>
    /// Gets the access token from local storage.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    public async Task<string> GetTokenAsync()
        => await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "accessToken");

    /// <summary>
    /// Sets the access token in local storage.
    /// </summary>
    /// <param name="token">The access token to set.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    public async Task SetTokenAsync(string token)
    {
        if (token == null)
        {
            await _jsRuntime.InvokeAsync<object>("localStorage.removeItem",
                                                            "accessToken");
        }
        else
        {
            await _jsRuntime.InvokeAsync<object>("localStorage.setItem",
                                                   "accessToken", token);
        }
    }
}