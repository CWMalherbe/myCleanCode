using Application.Interfaces;
using Infrastructure.Services.CurrentUserService;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API;

/// <summary>
/// Presentation building instructions for the Presentation layer. 
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    /// Presentation building instructions for the Presentation layer. 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}
