using System.ComponentModel.DataAnnotations;
using Application.Bases;
using Domain.Enums;

namespace Application.Entities.TruckParts;

/// <summary>
/// Represents a DTO (Data Transfer Object) for a truck part.
/// </summary>
public class TruckPartDTO : BaseEntityDTO
{
    /// <summary>
    /// Gets or sets the ID of the truck associated with the truck part.
    /// </summary>
    public long TruckId { get; set; }

    /// <summary>
    /// Gets or sets the name of the truck part.
    /// Maximum length is 64 characters.
    /// </summary>
    [Required]
    [StringLength(64, ErrorMessage = "Name has maximum length of 64 characters")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the code associated with the truck part.
    /// Maximum length is 128 characters.
    /// </summary>
    [Required]
    [StringLength(64, ErrorMessage = "Part Code has maximum length of 64 characters")]
    public string? Code { get; set; }

    /// <summary>
    /// Gets or sets the condition value of the truck part.
    /// </summary>
    public ConditionEnum ConditionValue { get; set; }

    /// <summary>
    /// Gets or sets the condition of the truck part.
    /// </summary>
    public string? Condition { get; set; }
}