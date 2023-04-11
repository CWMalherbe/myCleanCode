using Application.Exception;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Entities.TruckParts.Commands;
/// <summary>
/// Represents a command to delete a truck part from the database.
/// </summary>
public record DeleteTruckPartCommand : IRequest
{
    /// <summary>
    /// Gets or sets the ID of the truck part to be deleted.
    /// </summary>
    public long TruckPartId { get; init; }
}

/// <summary>
/// Handles the <see cref="DeleteTruckPartCommand"/> and deletes the corresponding truck part entity from the database.
/// </summary>
public class DeleteTruckPartCommandHandler : IRequestHandler<DeleteTruckPartCommand>
{
    private readonly IDatabaseManager<TruckPart> _databaseManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteTruckPartCommandHandler"/> class with the specified database manager.
    /// </summary>
    /// <param name="databaseManager">The database manager to use for accessing the database.</param>
    public DeleteTruckPartCommandHandler(IDatabaseManager<TruckPart> databaseManager)
    {
        _databaseManager = databaseManager;
    }

    /// <summary>
    /// Deletes the truck part entity with the specified ID from the database.
    /// </summary>
    /// <param name="request">The <see cref="DeleteTruckPartCommand"/> to handle.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="NotFoundException">Thrown if the truck part entity with the specified ID is not found in the database.</exception>
    public async Task Handle(DeleteTruckPartCommand request, CancellationToken cancellationToken)
    {
        TruckPart? existingEntity = await _databaseManager.ApplicationRepository.GetByIdAsync(request.TruckPartId, cancellationToken);
        if (existingEntity == null)
        {
            throw new NotFoundException(nameof(TruckPart), request.TruckPartId);
        }

        await _databaseManager.ApplicationRepository.DeleteAsync(existingEntity, cancellationToken);
        await _databaseManager.ApplicationRepository.SaveChangesAsync(cancellationToken);

    }
}