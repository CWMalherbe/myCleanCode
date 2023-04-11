using Application.Exception;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Entities.Trucks.Commands;

/// <summary>
/// Represents a command to delete a truck by ID.
/// </summary>
public record DeleteTruckCommand : IRequest
{
    /// <summary>
    /// Gets or initializes the ID of the truck to delete.
    /// </summary>
    public long TruckId { get; init; }
}

/// <summary>
/// Handles the <see cref="DeleteTruckCommand"/> by deleting the corresponding truck entity from the database.
/// </summary>
public class DeleteTruckCommandHandler : IRequestHandler<DeleteTruckCommand>
{
    private readonly IDatabaseManager<Truck> _databaseManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteTruckCommandHandler"/> class with the specified database manager.
    /// </summary>
    /// <param name="databaseManager">The database manager to use for accessing the truck entities.</param>
    public DeleteTruckCommandHandler(IDatabaseManager<Truck> databaseManager)
    {
        _databaseManager = databaseManager;
    }

    /// <summary>
    /// Deletes the truck entity with the ID specified in the given <paramref name="request"/> from the database.
    /// </summary>
    /// <param name="request">The request containing the ID of the truck to delete.</param>
    /// <param name="cancellationToken">The cancellation token to use for cancelling the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task Handle(DeleteTruckCommand request, CancellationToken cancellationToken)
    {
        // Retrieve the existing truck entity from the database
        Truck? existingEntity = await _databaseManager.ApplicationRepository.GetByIdAsync(request.TruckId, cancellationToken);

        // Check if the truck entity exists
        if (existingEntity == null)
        {
            throw new NotFoundException(nameof(existingEntity), request.TruckId);
        }

        // Delete the truck entity from the database and save the changes
        await _databaseManager.ApplicationRepository.DeleteAsync(existingEntity, cancellationToken);
        await _databaseManager.ApplicationRepository.SaveChangesAsync(cancellationToken);
    }
}
