using Application.Exception;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Entities.TruckParts.Queries;

/// <summary>
/// Query for getting a truck part.
/// </summary>
public record GetTruckPartQuery : IRequest<TruckPartDTO>
{
    /// <summary>
    /// The ID of the truck part to get.
    /// </summary>
    public long TruckPartId { get; init; }

    /// <summary>
    /// The ID of the truck the part belongs to.
    /// </summary>
    public long TruckId { get; init; }
}

/// <summary>
/// Handler for getting a truck part.
/// </summary>
public class GetTruckPartQueryHandler : IRequestHandler<GetTruckPartQuery, TruckPartDTO>
{
    private readonly IDatabaseManager<TruckPart> _databaseManager;
    private readonly IMapper<TruckPartDTO, TruckPart> _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetTruckPartQueryHandler"/> class.
    /// </summary>
    /// <param name="databaseManager">The database manager for truck parts.</param>
    /// <param name="mapper">The mapper for converting between DTOs and entities.</param>
    public GetTruckPartQueryHandler(IDatabaseManager<TruckPart> databaseManager, IMapper<TruckPartDTO, TruckPart> mapper)
    {
        _databaseManager = databaseManager;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to get a truck part.
    /// </summary>
    /// <param name="request">The query to get a truck part.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The truck part with the specified ID.</returns>
    /// <exception cref="NotFoundException">Thrown if the specified truck part is not found or does not belong to the specified truck.</exception>
    public async Task<TruckPartDTO> Handle(GetTruckPartQuery request, CancellationToken cancellationToken)
    {
        TruckPart? existingEntity = await _databaseManager.ApplicationRepository.GetByIdAsync(request.TruckPartId, cancellationToken);
        if (existingEntity == null || existingEntity.TruckId != request.TruckId)
        {
            throw new NotFoundException(nameof(TruckPart), request.TruckPartId);
        }
        return _mapper.MapEntityToDto(existingEntity);
    }
}
