using Application.Exception;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Entities.Trucks.Queries;

/// <summary>
/// A query to retrieve a truck by its ID.
/// </summary>
public record GetTruckQuery : IRequest<TruckDTO>
{
    /// <summary>
    /// The ID of the truck to retrieve.
    /// </summary>
    public long TruckId { get; init; }
}

/// <summary>
/// A handler to process a <see cref="GetTruckQuery"/> and retrieve the corresponding <see cref="TruckDTO"/>.
/// </summary>
public class GetTruckQueryHandler : IRequestHandler<GetTruckQuery, TruckDTO>
{
    private readonly IDatabaseManager<Truck> _databaseManager;
    private readonly IMapper<TruckDTO, Truck> _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetTruckQueryHandler"/> class.
    /// </summary>
    /// <param name="databaseManager">The database manager to use for retrieving truck entities.</param>
    /// <param name="mapper">The mapper to use for mapping truck entities to DTOs.</param>
    public GetTruckQueryHandler(IDatabaseManager<Truck> databaseManager, IMapper<TruckDTO, Truck> mapper)
    {
        _databaseManager = databaseManager;
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public async Task<TruckDTO> Handle(GetTruckQuery request, CancellationToken cancellationToken)
    {
        // Retrieve the truck entity with the specified ID.
        Truck? existingEntity = await _databaseManager.ApplicationRepository.GetByIdAsyncWithIncludes(request.TruckId, new string[] { nameof(Truck.Items) }, cancellationToken);

        if (existingEntity == null)
        {
            // If the truck entity is not found, throw a NotFoundException.
            throw new NotFoundException(nameof(existingEntity), request.TruckId);
        }

        // Map the truck entity to a DTO and return it.
        return _mapper.MapEntityToDto(existingEntity);
    }
}
