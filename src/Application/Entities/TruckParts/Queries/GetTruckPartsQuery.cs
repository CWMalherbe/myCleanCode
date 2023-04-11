using Application.Exception;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Application.Entities.TruckParts.Queries;

/// <summary>
/// Represents a query to retrieve the parts of a truck.
/// </summary>
public record GetTruckPartsQuery : IRequest<IList<TruckPartDTO>>
{
    /// <summary>
    /// Gets or initializes the ID of the truck to retrieve the parts for.
    /// </summary>
    public long TruckId { get; init; }
}

/// <summary>
/// Handles the <see cref="GetTruckPartsQuery"/> query by retrieving the corresponding truck entity and its parts from the database.
/// </summary>
public class GetTruckPartsQueryHandler : IRequestHandler<GetTruckPartsQuery, IList<TruckPartDTO>>
{
    private readonly IDatabaseManager<Truck> _databaseManager;
    private readonly IMapper<TruckPartDTO, TruckPart> _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetTruckPartsQueryHandler"/> class with the specified database manager and mapper.
    /// </summary>
    /// <param name="databaseManager">The database manager to use for accessing the truck entities.</param>
    /// <param name="mapper">The mapper to use for mapping between truck part entities and DTOs.</param>
    public GetTruckPartsQueryHandler(IDatabaseManager<Truck> databaseManager, IMapper<TruckPartDTO, TruckPart> mapper)
    {
        _databaseManager = databaseManager;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves the truck entity with the ID specified in the given <paramref name="request"/> from the database,
    /// maps its parts to DTOs using the configured mapper, and returns the resulting list of DTOs.
    /// </summary>
    /// <param name="request">The request containing the ID of the truck to retrieve the parts for.</param>
    /// <param name="cancellationToken">The cancellation token to use for cancelling the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation, whose result is the list of truck part DTOs.</returns>
    /// <exception cref="NotFoundException">Thrown if the truck entity with the specified ID is not found in the database.</exception>
    public async Task<IList<TruckPartDTO>> Handle(GetTruckPartsQuery request, CancellationToken cancellationToken)
    {
        // Retrieve the existing truck entity from the database
        Truck? existingEntity = await _databaseManager.ApplicationRepository.GetByIdAsyncWithIncludes(request.TruckId, new string[] { nameof(Truck.Items) }, cancellationToken);

        // Check if the truck entity exists
        if (existingEntity == null)
        {
            // If the truck entity is not found, throw a NotFoundException.
            throw new NotFoundException(nameof(existingEntity), request.TruckId);
        }

        // Map the truck entity's parts to DTOs using the configured mapper
        List<TruckPartDTO> entity = new List<TruckPartDTO>();
        if (existingEntity.Items != null)
        {
            entity = existingEntity.Items.Select(x => _mapper.MapEntityToDto(x)).ToList();
        }

        // Return the resulting list of DTOs
        return entity;
    }
}