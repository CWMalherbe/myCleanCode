using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Entities.Trucks.Queries;

/// <summary>
/// Get Truck query command object. 
/// Is empty as no pagination occurs. 
/// Should one want a truck by id, it could help to create a new get query with long id.
/// </summary>
public record GetAllTrucksQuery : IRequest<IList<TruckDTO>>
{

}

/// <summary>
/// Internal handler that executes the request.
/// </summary>
public class GetAllTrucksQueryHandler : IRequestHandler<GetAllTrucksQuery, IList<TruckDTO>>
{
    private readonly IDatabaseManager<Truck> _databaseManager;
    private readonly IMapper<TruckDTO, Truck> _mapper;


    /// <summary>
    /// Internal handler that executes the request.
    /// </summary>
    /// <param name="databaseManager"></param>
    /// <param name="mapper"></param>
    public GetAllTrucksQueryHandler(IMapper<TruckDTO, Truck> mapper, IDatabaseManager<Truck> databaseManager)
    {
        _databaseManager = databaseManager;
        _mapper = mapper;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IList<TruckDTO>> Handle(GetAllTrucksQuery request, CancellationToken cancellationToken)
    {
        return (await _databaseManager.ApplicationRepository.GetAllEntitiesAsync(cancellationToken)).Select(s => _mapper.MapEntityToDto(s)).ToList();
    }
}
