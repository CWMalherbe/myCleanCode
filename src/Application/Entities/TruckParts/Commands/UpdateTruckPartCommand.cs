using Application.Exception;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Entities.TruckParts.Commands;

/// <summary>
/// Represents a command to update an existing truck part.
/// </summary>
public record UpdateTruckPartCommand : IRequest
{
    /// <summary>
    /// Gets or sets the ID of the truck part to update.
    /// </summary>
    public long TruckPartId { get; init; }

    /// <summary>
    /// Gets or sets the ID of the truck associated with the part.
    /// </summary>
    public long TruckId { get; init; }

    /// <summary>
    /// Gets or sets the updated name of the truck part.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Gets or sets the updated code associated with the truck part.
    /// </summary>
    public string? Code { get; init; }

    /// <summary>
    /// Gets or sets the condition associated with the new part.
    /// </summary>
    public ConditionEnum Condition { get; init; }
}

/// <summary>
/// Internal handler responsible for updating the truck part.
/// </summary>
public class UpdateTruckPartHandler : IRequestHandler<UpdateTruckPartCommand>
{
    private readonly IDatabaseManager<TruckPart> _databaseManager;
    private readonly IMapper<TruckPartDTO, TruckPart> _mapper;
    private readonly IValidator<TruckPartDTO, TruckPart> _validator;

    /// <summary>
    /// Internal handler responsible for updating the truck part.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    /// <param name="validator"></param>
    public UpdateTruckPartHandler(IDatabaseManager<TruckPart> context, IMapper<TruckPartDTO, TruckPart> mapper, IValidator<TruckPartDTO, TruckPart> validator)
    {
        _databaseManager = context;
        _mapper = mapper;
        _validator = validator;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task Handle(UpdateTruckPartCommand request, CancellationToken cancellationToken)
    {
        //Should probably check if the truck exists or not
        //Find old entity
        TruckPart? existingEntity = await _databaseManager.ApplicationRepository.GetByIdAsync(request.TruckPartId, cancellationToken);
        if (existingEntity == null)
        {
            throw new NotFoundException(nameof(TruckPart), request.TruckPartId);
        }
        // Generate new entity
        TruckPart inputEntity = new TruckPart
        {
            Name = request.Name,
            Code = request.Code,
            TruckId = request.TruckId,
            Condition = request.Condition
        };
        _validator.ValidateEntity(inputEntity);
        //Update values
        existingEntity.Name = inputEntity.Name;
        //existingEntity.TruckId = inputEntity.TruckId;
        existingEntity.Code = inputEntity.Code;
        existingEntity.Condition = inputEntity.Condition;

        await _databaseManager.ApplicationRepository.UpdateAsync(existingEntity, cancellationToken);
        await _databaseManager.ApplicationRepository.SaveChangesAsync(cancellationToken);
    }
}
