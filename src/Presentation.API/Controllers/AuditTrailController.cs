using Application.Entities.AuditTrail.Queries.GetAuditTrails;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuditTrailEntity = Domain.Entities.AuditEntity;

namespace Presentation.API.Controllers;

/// <summary>
/// Controller for AuditTrails
/// </summary>
public class AuditTrailController : ApiControllerBase
{
    /// <summary>
    /// Silly controller to get all the audit logs
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IList<AuditTrailEntity>>> Get()
    {
        return (await Mediator.Send(new GetAuditTrailsQuery())).ToList();
    }
}
