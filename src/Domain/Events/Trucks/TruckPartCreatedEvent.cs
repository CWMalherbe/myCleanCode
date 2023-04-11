using Domain.Entities;

namespace Domain.Events.Trucks;
/// <summary>
/// Event to be called when a truck part is created.
/// </summary>
public class TruckPartCreatedEvent : BaseEvent
{
    /// <summary>
    /// Event to be called when a truck part is created.
    /// </summary>
    /// <param name="item"></param>
    public TruckPartCreatedEvent(TruckPart item)
    {
        Item = item;
    }
    /// <summary>
    /// Stores Item
    /// </summary>
    public TruckPart Item { get; }
}
