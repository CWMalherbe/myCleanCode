using System.Collections.Immutable;
using Application.Entities.Trucks;
using Application.Entities.Trucks.Commands;
using Application.Entities.Trucks.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Controllers;

/// <summary>
/// Controller for managing trucks.
/// </summary>
public class TruckController : ApiControllerBase
{
    /// <summary>
    /// Creates a new truck.
    /// </summary>
    /// <param name="command">The command to create a truck.</param>
    /// <returns>The ID of the new truck.</returns>
    [HttpPost]
    public async Task<ActionResult<long>> CreateTruck(CreateTruckCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Gets all trucks.
    /// </summary>
    /// <returns>A list of all trucks.</returns>
    [HttpGet]
    public async Task<ActionResult<IList<TruckDTO>>> GetAllTrucks()
    {
        return (await Mediator.Send(new GetAllTrucksQuery())).ToImmutableList();
    }

    /// <summary>
    /// Gets a specific truck by ID.
    /// </summary>
    /// <param name="TruckId">The ID of the truck to get.</param>
    /// <returns>The truck with the specified ID.</returns>
    [HttpGet("{TruckId}")]
    public async Task<ActionResult<TruckDTO>> GetTruck(long TruckId)
    {
        return await Mediator.Send(new GetTruckQuery { TruckId = TruckId });
    }

    /// <summary>
    /// Deletes a specific truck.
    /// </summary>
    /// <param name="TruckId">The id needed to delete a truck.</param>
    /// <returns>No content.</returns>
    [HttpDelete("{TruckId}")]
    public async Task<ActionResult> DeleteTruck(long TruckId)
    {
        await Mediator.Send(new DeleteTruckCommand { TruckId = TruckId });
        return Ok();
    }

    /// <summary>
    /// Updates a specific truck.
    /// </summary>
    /// <param name="command">The command to update a truck.</param>
    /// <returns>No content.</returns>
    [HttpPut]
    public async Task<ActionResult> UpdateTruck(UpdateTruckCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
}