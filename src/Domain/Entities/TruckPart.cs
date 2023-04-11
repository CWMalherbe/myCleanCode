namespace Domain.Entities;

/// <summary>
/// Represents a part of a truck.
/// </summary>
public class TruckPart : BaseAuditTrailEntity
{
    /// <summary>
    /// Gets or sets the ID of the truck that this part belongs to.
    /// </summary>
    public long TruckId { get; set; }

    /// <summary>
    /// Gets or sets the name of the truck part.
    /// </summary>
    /// <remarks>
    /// This property can be null if the truck part doesn't have a name.
    /// </remarks>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the condition of the truck part.
    /// </summary>
    public ConditionEnum Condition { get; set; }

    /// <summary>
    /// Gets or sets the code of the truck part.
    /// </summary>
    /// <remarks>
    /// This property can be null if the truck part doesn't have a code.
    /// </remarks>
    public string? Code { get; set; }
}
