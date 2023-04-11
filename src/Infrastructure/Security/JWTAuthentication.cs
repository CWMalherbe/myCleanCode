using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security;
/// <summary>
/// Implementaiton of JWT Authentication
/// </summary>
public class JWTAuthentication : IJWTAuthentication
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Implementation of JWT Authentication
    /// </summary>
    /// <param name="configuration"></param>
    public JWTAuthentication(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public string GenerateToken(int userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("TheAmazingDevelopmentSecretKey");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", userId.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [Obsolete("Use validate username instead")]
    public int ValidateToken(string token)
    {
        if (token == null)
            return 0;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("TheAmazingDevelopmentSecretKey");
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
            return userId;
        }
        catch
        {
            // return null if validation fails
            return 0;
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public string ValidateUsernameFromToken(string token)
    {
        string returnValue = "Unknown User";
        if (token != null)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                    ValidAudience = _configuration["Auth0:Audience"],
                    ValidIssuer = $"https://{_configuration["Auth0:Domain"]}"
                }, out SecurityToken validatedToken);

                JwtSecurityToken? jwtToken = (JwtSecurityToken)validatedToken;
                Claim? tempClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "Username");
                if (tempClaim != null)
                {
                    returnValue = tempClaim.Value;
                }
            }
            catch
            {
                // return null if validation fails
                // could add iformation here, but not quite needed
            }
        }
        return returnValue;
    }
}
