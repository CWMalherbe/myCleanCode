namespace Domain.Bases;

/// <summary>
/// Represents a base entity class for all database entities.
/// This class expands on the domain events of MediatR but is hidden from the database.
/// It's important to note that the Id property can only be a long type and should remain consistent across all entities.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the entity.
    /// </summary>
    public long Id { get; set; }

    private readonly List<BaseEvent> _domainEvents = new();

    /// <summary>
    /// Gets the collection of domain events associated with the entity.
    /// </summary>
    /// <returns>An immutable collection of domain events.</returns>
    public IReadOnlyCollection<BaseEvent> DomainEvents()
    {
        return _domainEvents.AsReadOnly();
    }

    /// <summary>
    /// Adds a domain event to the entity.
    /// </summary>
    /// <param name="domainEvent">The domain event to add.</param>
    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Removes a domain event from the entity.
    /// </summary>
    /// <param name="domainEvent">The domain event to remove.</param>
    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    /// <summary>
    /// Clears all domain events from the entity.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}