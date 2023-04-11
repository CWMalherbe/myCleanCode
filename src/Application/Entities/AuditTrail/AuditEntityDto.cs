using Application.Bases;
using Domain.Bases;

namespace Application.Entities.AuditTrail;


/// <summary>
/// Represents a data transfer object for an audit entity.
/// </summary>
public class AuditEntityDTO : BaseEntityDTO
{
    /// <summary>
    /// Gets or sets the date and time of the audit event.
    /// </summary>
    public DateTime DateTime { get; init; }

    /// <summary>
    /// Gets or sets the name of the database table associated with the audit event.
    /// </summary>
    public string? Table { get; init; }

    /// <summary>
    /// Gets or sets the name of the user associated with the audit event.
    /// </summary>
    public string? User { get; init; }

    /// <summary>
    /// Gets or sets the value associated with the audit event.
    /// </summary>
    public string? Value { get; init; }

    /// <summary>
    /// Gets or sets the change type associated with the audit event.
    /// </summary>
    public char Change { get; init; }

    /// <summary>
    /// Gets or sets the base entity associated with the audit event.
    /// </summary>
    public BaseEntity? Entity { get; init; }
}
