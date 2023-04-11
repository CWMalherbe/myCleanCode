using Application.Interfaces;
using Domain.Events.Trucks;
using MediatR;

namespace Application.Entities.TruckParts.EventHandlers;

/// <summary>
/// Event hanlder that implements INotificationHandler for MediatR. 
/// This is a very useful type of event handler, as one can do many things from alerts to 
/// executing stored procedures.
/// </summary>
public class TruckPartCreatedEventHandler : INotificationHandler<TruckPartCreatedEvent>
{
    private readonly ILogger _logger;

    /// <summary>
    /// Event hanlder that implements INotificationHandler for MediatR. 
    /// </summary>
    /// <param name="logger"></param>
    public TruckPartCreatedEventHandler(ILogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task Handle(TruckPartCreatedEvent notification, CancellationToken cancellationToken)
    {
        string eventName = notification.GetType().Name;
        _logger.Info("Domain Event: {eventName}", "TruckPartCreatedEventHandler.Handler", eventName);
        return Task.CompletedTask;
    }
}