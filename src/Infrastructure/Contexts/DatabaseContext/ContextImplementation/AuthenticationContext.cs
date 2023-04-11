using System.Reflection;
using Domain.Bases;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts.DatabaseContext.ContextImplementation;
/// <summary>
/// <inheritdoc/>
/// </summary>
public class AuthenticationContext : IdentityDbContext<BaseUser,IdentityRole<int>,int>
{
    private readonly IMediator _mediatR;

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="options"></param>
    /// <param name="mediatR"></param>
    public AuthenticationContext(DbContextOptions<AuthenticationContext> options, IMediator mediatR) : base(options)
    {
        _mediatR = mediatR;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        //builder.HasDefaultSchema("default");
        //builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //ADD INTERCEPTORS HERE
        //optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }
}
