using Application.Interfaces;
using Domain.Entities;

namespace Application.Entities.TruckParts;

/// <summary>
/// Truck part mapping between entity and application entity
/// </summary>
public class TruckPartMapping : IMapper<TruckPartDTO, TruckPart>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public TruckPart MapDtoToEntity(TruckPartDTO entity)
    {
        TruckPart returnValue = new TruckPart
        {
            Id = entity.Id,
            Name = entity.Name,
            Condition = entity.ConditionValue,
            TruckId = entity.TruckId,
            Code = entity.Code,
        };
        return returnValue;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public TruckPartDTO MapEntityToDto(TruckPart entity)
    {
        TruckPartDTO returnValue = new TruckPartDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            ConditionValue = entity.Condition,
            Condition = Enum.GetName(entity.Condition),
            TruckId = entity.TruckId,
            Code = entity.Code,
        };
        return returnValue;
    }
}