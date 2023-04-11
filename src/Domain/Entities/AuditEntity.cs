namespace Domain.Entities;

/// <summary>
/// Represents an entity for storing audit information in the database.
/// Inherits from <see cref="BaseEntity"/>.
/// </summary>
public class AuditEntity : BaseEntity
{
    /// <summary>
    /// Gets or sets the ID of the target entity that was audited.
    /// </summary>
    public long TargetId { get; set; }

    /// <summary>
    /// Gets or sets the date and time of the audit event.
    /// </summary>
    public DateTime DateTime { get; set; }

    /// <summary>
    /// Gets or sets the name of the table where the target entity is stored.
    /// </summary>
    public string? Table { get; set; }

    /// <summary>
    /// Gets or sets the name of the user who performed the audit action.
    /// </summary>
    public string? User { get; set; }

    /// <summary>
    /// Gets or sets the new value of the audited entity after the change.
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets the type of change that was made to the audited entity.
    /// </summary>
    public char Change { get; set; }
}

