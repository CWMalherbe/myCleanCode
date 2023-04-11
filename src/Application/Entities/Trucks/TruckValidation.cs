using Application.Extentions;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Entities.Trucks;

/// <summary>
/// Represents a validator for <see cref="TruckDTO"/> and <see cref="Truck"/> objects.
/// </summary>
public class TruckValidation : IValidator<TruckDTO, Truck>
{
    /// <inheritdoc />
    public void ValidateDtoEntity(TruckDTO DtoEntity)
    {
        // Validate the Name field of the TruckDto object.
        ValidationExtentions.CheckString64Field(DtoEntity.Name, typeof(TruckDTO).Name, nameof(DtoEntity.Name));
    }

    /// <inheritdoc />
    public void ValidateEntity(Truck entity)
    {
        // Validate the Name field of the TruckEntity object.
        ValidationExtentions.CheckString64Field(entity.Name, typeof(Truck).Name, nameof(entity.Name));
    }
}


