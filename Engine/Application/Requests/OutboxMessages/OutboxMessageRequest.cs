using Engine.Domain.Interfaces;

namespace Engine.Application.Requests.OutboxMessages;

public record OutboxMessageRequest(
    IDomainEvent Event,
    Guid AggregateId,
    string EventName,
    DateTime OccurredOn);
