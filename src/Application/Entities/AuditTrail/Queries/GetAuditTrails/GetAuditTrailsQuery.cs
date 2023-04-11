using Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AuditTrailEntity = Domain.Entities.AuditEntity;

namespace Application.Entities.AuditTrail.Queries.GetAuditTrails;
/// <summary>
/// Query that return all audit trails
/// </summary>
public record GetAuditTrailsQuery : IRequest<IList<AuditTrailEntity>>
{

}

/// <summary>
/// Internal handler that executes the request.
/// </summary>
public class GetAuditTrailsQueryHandler : IRequestHandler<GetAuditTrailsQuery, IList<AuditTrailEntity>>
{
    private readonly IDatabaseManager<AuditTrailEntity> _databaseManager;
    /// <summary>
    /// Internal handler that executes the request.
    /// </summary>
    /// <param name="databaseManager"></param>
    public GetAuditTrailsQueryHandler(IDatabaseManager<AuditTrailEntity> databaseManager)
    {
        _databaseManager = databaseManager;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IList<AuditTrailEntity>> Handle(GetAuditTrailsQuery request, CancellationToken cancellationToken)
    {
        return await _databaseManager.AuditRepository.Table.OrderByDescending(x => x.Id).ToListAsync();
    }
}
