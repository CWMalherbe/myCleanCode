using System.Text.Json;
using Application.Entities.AuditTrail;
using Application.Entities.AuditTrail.Commands.CreateAuditCommand;
using Domain.Bases;
using Infrastructure.EntityConfigurations;
using Infrastructure.Extentions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Contexts.DatabaseContext.ContextImplementation;
/// <summary>
/// <inheritdoc/>
/// </summary>
public class ApplicationContext : DbContext
{
    private readonly IMediator _mediatR;
    //USER to be replaced with getting the user.
    private const string USER = "PLACEHOLDER USER";

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="options"></param>
    /// <param name="mediatR"></param>
    public ApplicationContext(DbContextOptions<ApplicationContext> options, IMediator mediatR) : base(options)
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
        builder.ApplyConfiguration(new TruckConfiguration());
        builder.ApplyConfiguration(new TruckPartConfiguration());
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

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <returns></returns>
    public new DbSet<U> Set<U>() where U : BaseEntity
    {
        return base.Set<U>();
    }


    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="cancellationToken"></param>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {

        //I'm unsure if the following will cause race conditions in the db
        //Stress test is needed to find out.
        await _mediatR.DispatchDomainEvents(this);
        List<AuditEntityDTO> auditEntities = Audit();
        int result = await base.SaveChangesAsync(cancellationToken);
        await UpdateAuditTrail(auditEntities, cancellationToken);
        return result;
    }

    /// <summary>
    /// Scans for changes in audit based entities performs appropriate actions. 
    /// </summary>
    /// <returns>A list of Audit Entity Items to be saved</returns>
    private List<AuditEntityDTO> Audit()
    {
        DateTime currentTime = DateTime.UtcNow;
        List<AuditEntityDTO> auditEntities = new List<AuditEntityDTO>();
        ChangeTracker.DetectChanges();

        foreach (var entry in ChangeTracker.Entries<BaseAuditEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = USER;
                entry.Entity.Created = currentTime;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModifiedBy = USER;
                entry.Entity.LastModified = currentTime;
            }
        }

        foreach (var entry in ChangeTracker.Entries<BaseAuditTrailEntity>())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
            {
                AuditEntityDTO entity = new AuditEntityDTO
                {
                    DateTime = DateTime.UtcNow,
                    Table = GetMetaData(entry),
                    User = USER,
                    Value = JsonSerializer.Serialize(entry.CurrentValues.ToObject()),
                    Change = entry.State.ToString().First(),
                    Entity = entry.Entity,
                };
                auditEntities.Add(entity);
            }
        }
        return auditEntities;
    }

    /// <summary>
    /// Gets the Schema name and Table name
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    private static string GetMetaData(EntityEntry<BaseAuditTrailEntity> entry)
    {
        string returnValue = string.Empty;
        string? tempString;
        tempString = entry.Metadata.GetSchema();
        if (tempString != null)
        {
            returnValue += tempString + ".";
        }
        tempString = entry.Metadata.GetTableName();
        if (tempString != null)
        {
            returnValue += tempString;
        }
        return returnValue;
    }

    private async Task UpdateAuditTrail(List<AuditEntityDTO> auditEntities, CancellationToken token)
    {
        if (auditEntities.Any())
        {
            foreach (var auditEntity in auditEntities)
            {
                CreateAuditCommand command = new CreateAuditCommand
                {
                    DateTime = auditEntity.DateTime,
                    Table = auditEntity.Table,
                    User = auditEntity.User,
                    Change = auditEntity.Change,
                    Value = auditEntity.Value,
                    TargetId = auditEntity.Entity == null ? long.MinValue : auditEntity.Entity.Id
                };
                await _mediatR.Send(command, token);
            }
        }
    }
}
