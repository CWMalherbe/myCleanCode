using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Entities.AuditTrail.Commands.CreateAuditCommand;

/// <summary>
/// Represents a command to create an audit event.
/// </summary>
public record CreateAuditCommand : IRequest<long>
{
    /// <summary>
    /// Gets or sets the ID of the target entity associated with the audit event.
    /// </summary>
    public long TargetId { get; init; }

    /// <summary>
    /// Gets or sets the date and time of the audit event.
    /// </summary>
    public DateTime DateTime { get; init; }

    /// <summary>
    /// Gets or sets the name of the database table associated with the audit event.
    /// </summary>
    public string? Table { get; init; }

    /// <summary>
    /// Gets or sets the name of the user associated with the audit event.
    /// </summary>
    public string? User { get; init; }

    /// <summary>
    /// Gets or sets the value associated with the audit event.
    /// </summary>
    public string? Value { get; init; }

    /// <summary>
    /// Gets or sets the change type associated with the audit event.
    /// </summary>
    public char Change { get; init; }
}

/// <summary>
/// Creates an audity entry to send the the database. 
/// Currently sends to the same database. 
/// Still need to figure out how to send to a different Database, but for now should 
/// fine to use as it. Might be useful to have a secondary repository with secondary context
/// </summary>
public class CreateAuditCommandHandler : IRequestHandler<CreateAuditCommand, long>
{
    private readonly IDatabaseManager<AuditEntity> _databaseManager;

    /// <summary>
    /// Creates an audity entry to send the the database. 
    /// </summary>
    /// <param name="databaseManager"></param>
    public CreateAuditCommandHandler(IDatabaseManager<AuditEntity> databaseManager)
    {
        _databaseManager = databaseManager;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<long> Handle(CreateAuditCommand request, CancellationToken cancellationToken)
    {
        AuditEntity entity = new AuditEntity
        {
            TargetId = request.TargetId,
            Table = request.Table,
            User = request.User,
            Value = request.Value,
            DateTime = request.DateTime,
            Change = request.Change,
        };
        //Validate Entity

        await _databaseManager.AuditRepository.InsertAsync(entity, cancellationToken);
        await _databaseManager.AuditRepository.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}