using System.Collections.Immutable;
using Application.Entities.TruckParts;
using Application.Entities.TruckParts.Commands;
using Application.Entities.TruckParts.Queries;
using Application.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Controllers;


/// <summary>
/// Controller for managing truck parts.
/// </summary>
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TruckPartController : ApiControllerBase
{
    /// <summary>
    /// Creates a new truck part.
    /// </summary>
    /// <param name="command">The command to create the truck part.</param>
    /// <returns>The ID of the newly created truck part.</returns>
    [HttpPost]
    public async Task<ActionResult<long>> Create(CreateTruckPartCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// Retrieves alll the truck parts
    /// </summary>
    /// <returns>A list of truck parts.</returns>
    [HttpGet]
    public async Task<ActionResult<IList<TruckPartDTO>>> GetAllTruckParts()
    {
        return (await Mediator.Send(new GetAllTruckPartsQuery())).ToImmutableList();
    }

    /// <summary>
    /// Retrieves a paginated list of truck parts based on query parameters.
    /// </summary>
    /// <param name="query">The query parameters for retrieving truck parts.</param>
    /// <returns>A paginated list of truck parts.</returns>
    [HttpGet]
    [Route("Paginated")]
    public async Task<ActionResult<PaginatedList<TruckPartDTO>>> GetTruckPartsWithPagination([FromQuery] GetAllTruckPartsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Retrieves a list of truck parts for a specific truck.
    /// </summary>
    /// <param name="TruckId">The ID of the truck to retrieve parts for.</param>
    /// <returns>A list of truck parts for the specified truck.</returns>
    [HttpGet("{TruckId}")]
    public async Task<ActionResult<IList<TruckPartDTO>>> GetTruckParts(long TruckId)
    {
        return (await Mediator.Send(new GetTruckPartsQuery { TruckId = TruckId })).ToImmutableList();
    }

    /// <summary>
    /// Retrieves a specific truck part for a specific truck.
    /// </summary>
    /// <param name="TruckId">The ID of the truck that the truck part belongs to.</param>
    /// <param name="TruckPartId">The ID of the truck part to retrieve.</param>
    /// <returns>The specified truck part.</returns>
    [HttpGet("{TruckId}/{TruckPartId}")]
    public async Task<ActionResult<TruckPartDTO>> GetTruckPart(long TruckId, long TruckPartId)
    {
        return await Mediator.Send(new GetTruckPartQuery { TruckId = TruckId, TruckPartId = TruckPartId });
    }

    /// <summary>
    /// Deletes a specific truck part.
    /// </summary>
    /// <param name="TruckPartId"></param>
    /// <returns>200 Ok</returns>
    [HttpDelete("{TruckPartId}")]
    public async Task<ActionResult> DeleteTruckPart(long TruckPartId)
    {
        await Mediator.Send(new DeleteTruckPartCommand() { TruckPartId = TruckPartId });
        return Ok();
    }

    /// <summary>
    /// Updates a specific truck part.
    /// </summary>
    /// <param name="command"></param>
    /// <returns>200 Ok</returns>
    [HttpPut]
    public async Task<ActionResult> UpdateTruckPart(UpdateTruckPartCommand command)
    {
        await Mediator.Send(command);
        return Ok();
    }
}
