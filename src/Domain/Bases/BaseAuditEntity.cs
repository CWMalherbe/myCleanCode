namespace Domain.Bases;


/// <summary>
/// Represents a base entity that includes audit information such as creation and modification dates and the users who performed these actions.
/// </summary>
public abstract class BaseAuditEntity : BaseEntity
{
    /// <summary>
    /// Gets or sets the date and time the entity was created.
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Gets or sets the name of the user who created the entity.
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the date and time the entity was last modified.
    /// </summary>
    public DateTime? LastModified { get; set; }

    /// <summary>
    /// Gets or sets the name of the user who last modified the entity.
    /// </summary>
    public string? LastModifiedBy { get; set; }
}
