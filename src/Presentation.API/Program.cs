using Infrastructure;
using Infrastructure.Contexts.DatabaseContext.ContextImplementation;

namespace Presentation.API;

/// <summary>
/// Entry Point
/// </summary>
public class Program
{
    /// <summary>
    /// Entry Point
    /// No Arguments needed yet
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddAPIAuthentication(builder.Configuration);
        builder.Services.AddPresentationServices();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();

            // Initialise and seed database
            await Infrastructure.Extentions.ConfigureDatabaseExtention.InitializeDatabases<ApplicationContext>(app);
            await Infrastructure.Extentions.ConfigureDatabaseExtention.InitializeDatabases<AuditContext>(app);
            await Infrastructure.Extentions.ConfigureDatabaseExtention.InitializeDatabases<AuthenticationContext>(app);
        }
        else
        {
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        //ADD THIS PART IF YOU WOULD LIKE TO USE A WEBASSEMBLY
        app.UseBlazorFrameworkFiles();

        app.UseHealthChecks("/health");
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller}/{action=Index}/{id?}");

        app.Run();
    }
}
