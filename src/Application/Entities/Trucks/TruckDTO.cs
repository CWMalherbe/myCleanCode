using System.ComponentModel.DataAnnotations;
using Application.Bases;
using Application.Entities.TruckParts;

namespace Application.Entities.Trucks;

/// <summary>
/// Data transfer object (DTO) for a truck entity.
/// </summary>
public class TruckDTO : BaseEntityDTO
{
    /// <summary>
    /// Gets or sets the name of the truck.
    /// </summary>
    [Required]
    [StringLength(64, ErrorMessage = "Name has maximum length of 64 characters", MinimumLength = 1)]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the list of truck parts.
    /// </summary>
    public IList<TruckPartDTO>? Items { get; set; }

    /// <summary>
    /// Gets or sets the paint color of the truck.
    /// </summary>
    [Required]
    public string? Paint { get; set; }
}