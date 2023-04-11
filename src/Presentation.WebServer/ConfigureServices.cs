using Application.Interfaces;
using Infrastructure.Services.CurrentUserService;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using MudBlazor;
using MudBlazor.Services;
using Presentation.API.Controllers;
using Presentation.WebServer.Services;

namespace Presentation.WebServer;

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
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();

        //Code for adding an external assembly
        services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(ApiControllerBase).Assembly));

        // Add services to the container.
        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.NewestOnTop = false;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 10000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        });

        //AUTH STUFF
        // THIS IS AN INTERESTING PART. ONE SHOULD ADD ONE FOR EVERY DIFFERENT API SOURCE
        services.AddHttpClient<IHttpService, HttpService>();
        services.AddSingleton<IAccessBearer, AccessBearerService>();

        return services;
    }
}
