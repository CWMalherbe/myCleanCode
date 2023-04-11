using System.Security.Claims;
using System.Text;
using Application.Exception;
using Application.Interfaces;
using Domain.Bases;
using Infrastructure.Contexts.DatabaseContext;
using Infrastructure.Contexts.DatabaseContext.ContextImplementation;
using Infrastructure.Extentions;
using Infrastructure.Security;
using Infrastructure.Services.LoggingService;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;
/// <summary>
/// Infrastructure building instructions for the Infrastructure layer. 
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    /// Infrastructure building instructions for the Infrastructure layer. 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        ConfigureDatabaseExtention.AddDbContext<ApplicationContext>(services, configuration);
        ConfigureDatabaseExtention.AddDbContext<AuditContext>(services, configuration);
        ConfigureDatabaseExtention.AddDbContext<AuthenticationContext>(services, configuration);

        services.AddSingleton<ILogger>(new LoggingService(Factory.LoggingFactory.GetSeriLogger(), configuration));

        services
            .AddIdentityCore<BaseUser>()
            .AddRoles<IdentityRole<int>>()
            .AddEntityFrameworkStores<AuthenticationContext>();

        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<IJWTAuthentication, JWTAuthentication>();

        services.AddScoped(typeof(IDatabaseManager<>), typeof(DatabaseManager<>));

        return services;
    }

    /// <summary>
    /// Because both the API and the WebServices makes use Jwt, I made a single source
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    private static Action<JwtBearerOptions> JwtBearerOptions(IConfiguration configuration)
    {
        return options =>
        {
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    return Task.CompletedTask;
                },
            };
            options.Authority = $"https://{configuration["Auth0:Domain"]}";
            options.Audience = configuration["Auth0:Audience"];
            options.IncludeErrorDetails = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = ClaimTypes.NameIdentifier,
                ValidAudience = configuration["Auth0:Audience"],
                ValidIssuer = $"https://{configuration["Auth0:Domain"]}"
            };
        };
    }


    /// <summary>
    /// Generalised snippet of the code needed to run JwtAuthentication with 
    /// a user hybrid system.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddAPIAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        //var key = Encoding.ASCII.GetBytes("TheAmazingDevelopmentSecretKey");
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, JwtBearerOptions(configuration));
        return services;
    }

    /// <summary>
    /// Adds the wep app authentication. 
    /// Pass the service and configuration. 
    /// This Authentication system uses both the cookie and openid connect. 
    /// The openid connect tries to authenticat with auth0
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddWebAPPAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddAuthorizationCore();
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
            {
                config.Cookie.Name = "aculocity.cookie";
                config.LoginPath = configuration["Auth0:LoginPath"];
                config.LogoutPath = "/";
                config.Events = new CookieAuthenticationEvents()
                {
                    OnSignedIn = (context) =>
                    {
                        return Task.CompletedTask;
                    }
                };
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, JwtBearerOptions(configuration))
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {

                options.Authority = $"https://{configuration["Auth0:Domain"]}";
                options.ClientId = configuration["Auth0:ClientId"];
                options.ClientSecret = configuration["Auth0:ClientSecret"];
                options.ResponseType = OpenIdConnectResponseType.CodeIdTokenToken;
                options.Scope.Clear();
                options.Scope.Add("openid email nickname name username roles psid profile");
                options.CallbackPath = new PathString("/callback");
                options.ClaimsIssuer = "Auth0";
                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                    ValidAudience = configuration["Auth0:Audience"],
                    ValidIssuer = $"https://{configuration["Auth0:Domain"]}"
                };
                options.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProviderForSignOut = (context) =>
                    {
                        var logoutUri = $"https://{configuration["Auth0:Domain"]}/v2/logout?client_id={configuration["Auth0:ClientId"]}";
                        var postLogoutUri = context.Properties.RedirectUri;
                        if (!string.IsNullOrEmpty(postLogoutUri))
                        {
                            if (postLogoutUri.StartsWith("/"))
                            {
                                var request = context.Request;
                                postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase;
                            }
                            logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
                        }

                        context.Response.Redirect(logoutUri);
                        context.HandleResponse();
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = async (context) =>
                    {
                        //THIS SECTION IS A MASSIVE HACK, NOT QUITE SURE HOW TO SOLVE THE ISSUE
                        if (context.Principal != null)
                        {
                            //CALL THIS TO GET LEGACY USERS ROLES
                            //UNTESTED WITH THE PRODUCTION SYSTEM
                            if (context.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role) == null) 
                            {
                                await SecurityExtentions.ExtractRolesFromExternalSource(context);
                            }

                            //CALL THIS TO GET THE ACCESS TOKEN
                            var identity = new ClaimsIdentity(new Claim[] { new Claim("access_token", context.SecurityToken.RawData) });
                            if (context.Principal != null)
                            {
                                context.Principal.AddIdentity(identity);
                            }
                        }
                    },
                    OnAccessDenied = context =>
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/");
                        return Task.CompletedTask;
                    }
                };
            });
        return services;
    }
}
