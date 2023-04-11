using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Entities.TruckParts.Queries;

/// <summary>
/// Query for getting all truck parts.
/// </summary>
public record GetAllTruckPartsQuery : IRequest<IList<TruckPartDTO>>
{
}

/// <summary>
/// Handler for getting all truck parts.
/// </summary>
public class GetAllTruckPartsQueryHandler : IRequestHandler<GetAllTruckPartsQuery, IList<TruckPartDTO>>
{
    private readonly IDatabaseManager<TruckPart> _databaseManager;
    private readonly IMapper<TruckPartDTO, TruckPart> _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllTruckPartsQueryHandler"/> class.
    /// </summary>
    /// <param name="databaseManager">The database manager for truck parts.</param>
    /// <param name="mapper">The mapper for converting between DTOs and entities.</param>
    public GetAllTruckPartsQueryHandler(IDatabaseManager<TruckPart> databaseManager, IMapper<TruckPartDTO, TruckPart> mapper)
    {
        _databaseManager = databaseManager;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the request to get all truck parts.
    /// </summary>
    /// <param name="request">The query to get all truck parts.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A list of all truck parts.</returns>
    public async Task<IList<TruckPartDTO>> Handle(GetAllTruckPartsQuery request, CancellationToken cancellationToken)
    {
        return (await _databaseManager.ApplicationRepository.GetAllEntitiesAsync(cancellationToken))
            .Select(x => _mapper.MapEntityToDto(x))
            .ToList();
    }
}