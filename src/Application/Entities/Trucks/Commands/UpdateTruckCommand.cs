using Application.Exception;
using Application.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;

namespace Application.Entities.Trucks.Commands;

/// <summary>
/// Represents a command to update a truck.
/// </summary>
public record UpdateTruckCommand : IRequest
{
    /// <summary>
    /// Gets or sets the new name for the truck.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Gets or sets the new paint color for the truck.
    /// </summary>
    public string? Paint { get; init; }

    /// <summary>
    /// Gets or sets the ID of the truck to update.
    /// </summary>
    public long TruckId { get; init; }
}

/// <summary>
/// Represents a handler for the <see cref="UpdateTruckCommand"/>.
/// </summary>
public class UpdateTruckCommandHandler : IRequestHandler<UpdateTruckCommand>
{
    private readonly IDatabaseManager<Truck> _databaseManager;
    private readonly IValidator<TruckDTO, Truck> _validator;

    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateTruckCommandHandler"/> class.
    /// </summary>
    /// <param name="databaseManager">The database manager.</param>
    /// <param name="validator">The validator.</param>
    public UpdateTruckCommandHandler(IDatabaseManager<Truck> databaseManager, IValidator<TruckDTO, Truck> validator)
    {
        _databaseManager = databaseManager;
        _validator = validator;
    }

    /// <summary>
    /// Handles the update truck command.
    /// </summary>
    /// <param name="request">The update truck command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task Handle(UpdateTruckCommand request, CancellationToken cancellationToken)
    {
        Truck? existingEntity = await _databaseManager.ApplicationRepository.GetByIdAsync(request.TruckId, cancellationToken);
        if (existingEntity == null)
        {
            throw new NotFoundException(nameof(existingEntity), request.TruckId);
        }
        Truck entity = new Truck
        {
            Name = request.Name,
            Paint = string.IsNullOrEmpty(request.Paint) ? Paint.Unkown : Paint.From(request.Paint),
        };
        _validator.ValidateEntity(entity);

        existingEntity.Name = entity.Name;
        existingEntity.Paint = entity.Paint;

        await _databaseManager.ApplicationRepository.UpdateAsync(existingEntity, cancellationToken);
        await _databaseManager.ApplicationRepository.SaveChangesAsync(cancellationToken);
    }
}