using Engine.Application.Interfaces;
using Engine.Domain.Interfaces;
using System.Text.Json;

namespace Engine.Application.Common.Serialization
{
    public class EventDeserializer : IEventDeserializerService
    {
        public TEvent Deserialize<TEvent>(string eventTypeName, string jsonData) where TEvent : IDomainEvent
        {
            if (string.IsNullOrWhiteSpace(eventTypeName))
            {
                throw new ArgumentException("Event type name cannot be null or empty.", nameof(eventTypeName));
            }
            if (string.IsNullOrWhiteSpace(jsonData))
            {
                throw new ArgumentException("JSON data cannot be null or empty.", nameof(jsonData));
            }

            Type? eventType = Type.GetType(eventTypeName);

            if (eventType == null)
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    eventType = assembly.GetType(eventTypeName);
                    if (eventType != null) break;
                }

                if (eventType == null)
                {
                    throw new InvalidOperationException($"Could not resolve event type: '{eventTypeName}'. Ensure the type name is correct and the assembly is loaded.");
                }
            }

            object? deserializedObject;
            try
            {
                deserializedObject = JsonSerializer.Deserialize(jsonData, eventType);
            }
            catch (JsonException ex)
            {
                //_logger.LogError(ex, "JSON deserialization failed for event type '{EventTypeName}' with data: {JsonData}", eventTypeName, jsonData);
                throw new InvalidOperationException($"Failed to deserialize JSON for event type '{eventTypeName}'. See inner exception for details.", ex);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "An unexpected error occurred during deserialization for event type '{EventTypeName}'.", eventTypeName);
                throw new InvalidOperationException($"An unexpected error occurred during deserialization for event type '{eventTypeName}'.", ex);
            }

            if (deserializedObject is not TEvent resultEvent)
            {
                throw new InvalidOperationException
                    ($"Failed to deserialize JSON to expected event type '{typeof(TEvent).Name}'. Deserialized type was '{deserializedObject?.GetType().Name ?? "null"}' for event type name '{eventTypeName}'.");
            }

            return resultEvent;
        }
    }
}
