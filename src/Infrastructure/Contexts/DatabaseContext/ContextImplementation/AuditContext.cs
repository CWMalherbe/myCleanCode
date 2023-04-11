using System.Reflection;
using Domain.Entities;
using Infrastructure.EntityConfigurations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts.DatabaseContext.ContextImplementation;
/// <summary>
/// <inheritdoc/>
/// </summary>
public class AuditContext : DbContext
{
    private readonly IMediator _mediatR;
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="options"></param>
    /// <param name="mediatR"></param>
    public AuditContext(DbContextOptions<AuditContext> options, IMediator mediatR) : base(options)
    {
        _mediatR = mediatR;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        //builder.HasDefaultSchema("default");
        builder.ApplyConfiguration(new AuditEntityConfiguration());
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
