using Application.Entities.TruckParts;
using Application.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Entities.Trucks;

/// <summary>
/// Represents a mapper for converting between <see cref="TruckDTO"/> and <see cref="Truck"/>.
/// </summary>
public class TruckMapping : IMapper<TruckDTO, Truck>
{
    /// <summary>
    /// The mapper used for converting between <see cref="TruckPartDTO"/> and <see cref="TruckPart"/>.
    /// </summary>
    private readonly IMapper<TruckPartDTO, TruckPart> _mapperItem;

    /// <summary>
    /// Initializes a new instance of the <see cref="TruckMapping"/> class.
    /// </summary>
    /// <param name="mapperItem">The mapper used for converting between <see cref="TruckPartDTO"/> and <see cref="TruckPart"/>.</param>
    public TruckMapping(IMapper<TruckPartDTO, TruckPart> mapperItem)
    {
        _mapperItem = mapperItem;
    }

    /// <inheritdoc />
    public Truck MapDtoToEntity(TruckDTO entity)
    {
        // Convert a TruckDTO object to a Truck object.
        Truck returnValue = new Truck
        {
            Id = entity.Id,
            Name = entity.Name,
            Paint = string.IsNullOrEmpty(entity.Paint) ? Paint.Unkown : Paint.From(entity.Paint),
            Items = entity.Items != null ? entity.Items.Select(s => _mapperItem.MapDtoToEntity(s)).ToList() : new List<TruckPart>()
        };
        return returnValue;
    }

    /// <inheritdoc />
    public TruckDTO MapEntityToDto(Truck entity)
    {
        // Convert a Truck object to a TruckDTO object.
        TruckDTO returnValue = new TruckDTO
        {
            Id = entity.Id,
            Name = entity.Name,
            Paint = entity.Paint == null ? Paint.Unkown.Hex : entity.Paint.Hex,
            Items = entity.Items != null ? entity.Items.Select(s => _mapperItem.MapEntityToDto(s)).ToList() : new List<TruckPartDTO>()
        };
        return returnValue;
    }
}

