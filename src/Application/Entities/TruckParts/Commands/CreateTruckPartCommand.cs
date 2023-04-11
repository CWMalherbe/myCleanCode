using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Events.Trucks;
using MediatR;

namespace Application.Entities.TruckParts.Commands;

/// <summary>
/// Represents a command to create a new truck part.
/// </summary>
public record CreateTruckPartCommand : IRequest<long>
{
    /// <summary>
    /// Gets or sets the ID of the truck associated with the new part.
    /// </summary>
    public long TruckId { get; init; }

    /// <summary>
    /// Gets or sets the name of the new part.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Gets or sets the code associated with the new part.
    /// </summary>
    public string? Code { get; init; }

    /// <summary>
    /// Gets or sets the condition associated with the new part.
    /// </summary>
    public ConditionEnum Condition { get; init; }
}

/// <summary>
/// Internal create truck part command handler
/// </summary>
public class CreateTruckPartCommandHandler : IRequestHandler<CreateTruckPartCommand, long>
{
    private readonly IDatabaseManager<TruckPart> _databaseManager;
    private readonly IValidator<TruckPartDTO, TruckPart> _validator;

    /// <summary>
    /// Internal create truck part command handler
    /// </summary>
    /// <param name="databaseManager"></param>
    /// <param name="validator"></param>
    public CreateTruckPartCommandHandler(IDatabaseManager<TruckPart> databaseManager, IValidator<TruckPartDTO, TruckPart> validator)
    {
        _databaseManager = databaseManager;
        _validator = validator;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<long> Handle(CreateTruckPartCommand request, CancellationToken cancellationToken)
    {
        TruckPart entity = new TruckPart
        {
            Name = request.Name,
            Code = request.Code,
            TruckId = request.TruckId,
            Condition = request.Condition
        };
        _validator.ValidateEntity(entity);

        //Notice the use of adding a domain event.
        entity.AddDomainEvent(new TruckPartCreatedEvent(entity));

        await _databaseManager.ApplicationRepository.InsertAsync(entity, cancellationToken);
        await _databaseManager.ApplicationRepository.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}