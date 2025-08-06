using Engine.Domain.Interfaces;

namespace Engine.Application.Interfaces;

public interface IEventDeserializerService
{
    /// <summary>
    /// Deserializes the event data from a JSON string.
    /// </summary>
    /// <param name="data">The JSON string containing the event data.</param>
    /// <typeparam name="TEvent">The type of the event to deserialize.</typeparam>
    /// <returns>An instance of the event type.</returns>
    TEvent Deserialize<TEvent>(string eventType, string data) where TEvent : IDomainEvent;
}
