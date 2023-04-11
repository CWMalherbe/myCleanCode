using System.ComponentModel.DataAnnotations;

namespace Application.Bases;

/// <summary>
/// BaseEntityDto for Application. 
/// Important to note that Id can only be a long. Can be altered later, but important 
/// that all Ids remain the same type.
/// Currently, the Id as set as required. Not sure if we will need it as such. 
/// Remember to include annotations to your entities.
/// </summary>
public partial class BaseEntityDTO
{
    /// <summary>
    /// Id to be used for all entities. Please check Class comments for more details
    /// </summary>
    [Required]
    public long Id { get; init; }
}
