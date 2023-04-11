using Domain.Bases;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Extentions;
/// <summary>
/// Extentions for mediatR
/// </summary>
public static class MediatRExtensions
{
    /// <summary>
    /// Function that allows us to publish domain events for all the entities actioned.
    /// It also clears all the events from the entities.
    /// Unsure what would happen should the server crash.
    /// </summary>
    /// <param name="mediatR"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static async Task DispatchDomainEvents(this IMediator mediatR, DbContext context)
    {
        var entities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents().Any())
            .Select(e => e.Entity);
        var domainEvents = entities
            .SelectMany(e => e.DomainEvents())
            .ToList();
        entities.ToList().ForEach(e => e.ClearDomainEvents());
        foreach (var domainEvent in domainEvents)
            await mediatR.Publish(domainEvent);
    }
}
