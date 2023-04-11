namespace Domain.Entities;

/// <summary>
/// Represents a truck entity with audit trail support.
/// </summary>
public class Truck : BaseAuditTrailEntity
{
    /// <summary>
    /// Gets or sets the name of the truck.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the list of items associated with the truck.
    /// </summary>
    public IList<TruckPart>? Items { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Truck"/> class.
    /// </summary>
    public Truck()
    {
        Items = new List<TruckPart>();
        Paint = Paint.Unkown;
    }

    /// <summary>
    /// Gets or sets the paint of the truck.
    /// </summary>
    public Paint? Paint { get; set; }
}
