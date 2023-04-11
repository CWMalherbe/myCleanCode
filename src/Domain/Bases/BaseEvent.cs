using MediatR;

namespace Domain.Bases;
/// <summary>
/// Base event class that implements the INotification from MediatR
/// Use to create entity events.
/// </summary>
public abstract class BaseEvent : INotification
{
}
