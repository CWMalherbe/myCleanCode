using Application.Interfaces;
using Domain.Bases;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts.DatabaseContext;
/// <summary>
/// Initialised a specific database
/// </summary>
public class DatabaseInitialiser<T> where T : DbContext
{
    private readonly ILogger _logger;
    private readonly DbContext _context;

    /// <summary>
    /// Initialised a specific database
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="context"></param>
    public DatabaseInitialiser(ILogger logger, T context)
    {
        _logger = logger;
        _context = context;
    }

    /// <summary>
    /// Performs Migrations at the start. 
    /// If one does not want migrations, one should not add this to the startup. 
    /// </summary>
    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
            else if (_context.Database.IsNpgsql())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.Error("An error occurred while initialising the database: {ex}", "ApplicationDbContextInitialiser.InitialiseAsync", ex);
            throw;
        }
    }
}
