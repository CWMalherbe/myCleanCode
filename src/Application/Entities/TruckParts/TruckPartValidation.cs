using Application.Extentions;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Entities.TruckParts;

/// <summary>
/// Validates the truck parts
/// </summary>
public class TruckPartValidation : IValidator<TruckPartDTO, TruckPart>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="DtoEntity"></param>
    public void ValidateDtoEntity(TruckPartDTO DtoEntity)
    {
        ValidationExtentions.CheckString64Field(DtoEntity.Name, typeof(TruckPartDTO).Name, nameof(DtoEntity.Name));
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="entity"></param>
    public void ValidateEntity(TruckPart entity)
    {
        ValidationExtentions.CheckString64Field(entity.Name, typeof(TruckPartDTO).Name, nameof(entity.Name));
    }
}
