using System.Net.Http.Headers;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Application.Security.Commands.Authorization;
/// <summary>
/// Command for authentication by means of Auth0
/// </summary>
public record AuthenticateUserCommand : IRequest<string>
{
    /// <summary>
    /// Normal username
    /// </summary>
    public string? Username { get; init; }
    /// <summary>
    /// Normal password
    /// </summary>
    public string? Password { get; init; }
}

/// <summary>
/// Command for authentication by means of Auth0
/// </summary>
public class AuthenticateUserCommandHandler : IRequestHandler<AuthenticateUserCommand, string>
{
    private readonly IJWTAuthentication _jwtAuthentication;
    private readonly IConfiguration _configuration;
    private readonly IIdentityService _identityService;


    /// <summary>
    /// Command for authentication by means of Auth0
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="jwtAuthentication"></param>
    /// <param name="identityService"></param>
    public AuthenticateUserCommandHandler(IConfiguration configuration, IJWTAuthentication jwtAuthentication, IIdentityService identityService)
    {
        _configuration = configuration;
        _jwtAuthentication = jwtAuthentication;
        _identityService = identityService;
    }

    /// <summary>
    /// <inheritdoc/>
    /// This code is a giantic mess, but o well
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<string> Handle(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        if (request.Username == null || request.Password == null) { return ""; }

        var access_token = string.Empty;
        using (HttpClient client = new HttpClient())
        {

#pragma warning disable CS8604 // Possible null reference argument.
            var values = new Dictionary<string, string>()
            {
                { "grant_type", "password"},
                { "username", request.Username},
                { "password", request.Password},
                { "scope", "openid name email profile"},
                { "client_id", _configuration["Auth0:ClientId"]},
                { "client_secret", _configuration["Auth0:ClientSecret"]},
                { "audience", _configuration["Auth0:Audience"]},
            };
#pragma warning restore CS8604 // Possible null reference argument.

            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync($"https://{_configuration["Auth0:Domain"]}/oauth/token", content);
            var responseString = await response.Content.ReadAsStringAsync();
            access_token = JObject.Parse(responseString)["access_token"]?.ToString();
        }

        JObject? userObject;
        using (HttpClient client = new HttpClient())
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://{_configuration["Auth0:Domain"]}/userinfo"))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access_token);

                var response = await client.SendAsync(requestMessage);
                var responseString = await response.Content.ReadAsStringAsync();
                userObject = JObject.Parse(responseString);
            }
        }

        if (userObject != null && userObject.ContainsKey("sub"))
        {
            JToken? token = userObject.GetValue("sub");
            if (token != null)
            {
                int userId = await _identityService.GetUserIdByIdentification(token.ToString());
                if (userId > 0)
                {
                    return _jwtAuthentication.GenerateToken(userId);
                }
            }
        }

        return "";
    }
}
