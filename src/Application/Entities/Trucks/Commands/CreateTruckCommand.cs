using Application.Entities.TruckParts;
using Application.Entities.TruckParts.Commands;
using Application.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;

namespace Application.Entities.Trucks.Commands;

/// <summary>
/// Represents a command for creating a new truck.
/// </summary>
public record CreateTruckCommand : IRequest<long>
{
    /// <summary>
    /// Gets or sets the name of the truck.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Gets or sets the list of commands for creating truck parts associated with the truck.
    /// </summary>
    public IList<CreateTruckPartCommand>? Items { get; init; }

    /// <summary>
    /// Gets or sets the paint color of the truck.
    /// </summary>
    public string? Paint { get; init; }
}

/// <summary>
/// Create Truck Command Handler that handles the create command request.
/// </summary>
public class CreateTruckCommandHandler : IRequestHandler<CreateTruckCommand, long>
{
    private readonly IDatabaseManager<Truck> _databaseManager;
    private readonly IMapper<TruckDTO, Truck> _mapper;
    private readonly IValidator<TruckDTO, Truck> _validatorTruck;
    private readonly IValidator<TruckPartDTO, TruckPart> _validatorTruckPart;

    /// <summary>
    /// Create Truck Command Handler that handles the create command request.
    /// </summary>
    /// <param name="databaseManager"></param>
    /// <param name="mapper"></param>
    /// <param name="validatorTruck"></param>
    /// <param name="validatorTruckPart"></param>
    public CreateTruckCommandHandler(IDatabaseManager<Truck> databaseManager, IMapper<TruckDTO, Truck> mapper, IValidator<TruckDTO, Truck> validatorTruck, IValidator<TruckPartDTO, TruckPart> validatorTruckPart)
    {
        _databaseManager = databaseManager;
        _mapper = mapper;
        _validatorTruck = validatorTruck;
        _validatorTruckPart = validatorTruckPart;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<long> Handle(CreateTruckCommand request, CancellationToken cancellationToken)
    {
        Truck entity = new Truck
        {
            Name = request.Name,
            Paint = string.IsNullOrEmpty(request.Paint) ? Paint.Unkown : Paint.From(request.Paint),
            Items = request.Items != null ? request.Items.Select(x =>
            {
                TruckPart entityPart = new TruckPart
                {
                    Name = x.Name,
                    Code = x.Code,
                    Condition= x.Condition,
                };
                _validatorTruckPart.ValidateEntity(entityPart);
                return entityPart;
            }).ToList() : new List<TruckPart>()
        };
        _validatorTruck.ValidateEntity(entity);

        await _databaseManager.ApplicationRepository.InsertAsync(entity, cancellationToken);
        await _databaseManager.ApplicationRepository.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}

