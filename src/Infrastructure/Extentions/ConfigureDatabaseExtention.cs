using System.Reflection;
using Application.Exception;
using Infrastructure.Contexts.DatabaseContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extentions;
/// <summary>
/// Helping functions that allow for the database configureations
/// </summary>
public class ConfigureDatabaseExtention
{
    /// <summary>
    /// Adds specific database context
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <exception cref="NotFoundException"></exception>
    public static void AddDbContext<T>(IServiceCollection services, IConfiguration configuration) where T : DbContext
    {
        string dbName = typeof(T).Name;
        string? assembly = Assembly.GetEntryAssembly()?.GetName().Name;
        //assembly = "Presentation.WebServer";
        if (configuration.GetValue<bool>($"{dbName}:UseInMemoryDatabase"))
        {
            services.AddDbContext<T>(options =>
                options.UseInMemoryDatabase("InMemoryDb"));
        }
        else
        {
            string? databaseType = configuration.GetValue<string>($"{dbName}:DatabaseType");
            switch (databaseType)
            {
                case "Postgres":
                    services.AddDbContext<T>(options =>
                        options.UseNpgsql(configuration.GetValue<string>($"{dbName}:Connection"),
                        b => b.MigrationsAssembly(assembly)));
                    break;
                case "SQLServer":
                    services.AddDbContext<T>(options =>
                        options.UseSqlServer(configuration.GetValue<string>($"{dbName}:Connection"),
                           b => b.MigrationsAssembly(assembly)));
                    break;
                default:
                    throw new NotFoundException("Database type not recognised");
            }

        }
        services.AddHealthChecks().AddDbContextCheck<T>();
        services.AddScoped<DatabaseInitialiser<T>>();
    }

    /// <summary>
    /// Initializes the specific databases
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="app"></param>
    /// <returns></returns>
    public static async Task InitializeDatabases<T>(WebApplication app) where T : DbContext
    {
        using (var scope = app.Services.CreateScope())
        {
            var initialiser = scope.ServiceProvider.GetRequiredService<DatabaseInitialiser<T>>();
            await initialiser.InitialiseAsync();
        }
    }
}
