using System.Net.NetworkInformation;
using System.Reflection;
using Application.Entities.TruckParts;
using Application.Entities.Trucks;
using Application.Interfaces;
using Application.Middleware;
using Domain.Entities;
using MediatR;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Application building instructions for the Application layer. 
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //MediatR
        //services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        //All the entity mappings and validators
        services.AddTransient<IValidator<TruckDTO, Truck>, TruckValidation>();
        services.AddTransient<IValidator<TruckPartDTO, TruckPart>, TruckPartValidation>();
        services.AddTransient<IMapper<TruckDTO, Truck>, TruckMapping>();
        services.AddTransient<IMapper<TruckPartDTO, TruckPart>, TruckPartMapping>();

        //Can easily be replaced with microsoft middleware functions, using MediatR as it is modern
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionMiddleware<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationMiddleware<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceMiddleware<,>));


        return services;
    }
}