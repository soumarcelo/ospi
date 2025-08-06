using Engine.Domain.Enums;
using Engine.Domain.Interfaces;
using System.Text.Json;

namespace Engine.Domain.Entities;

public class OutboxMessage
{
    public Guid Id { get; private set; }
    public Guid AggregateId { get; private set; }
    public string MessageType { get; private set; }
    public string Data { get; private set; }
    public OutboxMessageStatus Status { get; private set; }
    public int Attempts { get; private set; }
    public OutboxMessageFailureReasonCode? FailureReasonCode { get; private set; }
    public string? FailureReasonDescription { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime FailedAt { get; private set; }
    public DateTime OccurredOn { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public OutboxMessage()
    {
        MessageType = string.Empty;
        Data = string.Empty;
    }

    public static OutboxMessage Create(
        IDomainEvent data, 
        Guid aggregateId,
        string eventTypeName, 
        DateTime occurredOn)
    {
        string json = JsonSerializer.Serialize(data);

        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            AggregateId = aggregateId,
            MessageType = eventTypeName,
            Data = json,
            Status = OutboxMessageStatus.Pending,
            Attempts = 0,
            CreatedAt = DateTime.UtcNow,
            OccurredOn = occurredOn
        };
    }

    public void Publish()
    {
        if (Status != OutboxMessageStatus.Pending)
        {
            throw new InvalidOperationException("Only pending messages can be published.");
        }

        Status = OutboxMessageStatus.Published;
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncrementAttempts()
    {
        Attempts++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Failure(OutboxMessageFailureReasonCode reasonCode, string reasonDescription)
    {
        if (Status == OutboxMessageStatus.Published)
        {
            throw new InvalidOperationException("Cannot mark a published message as failed.");
        }

        Status = OutboxMessageStatus.Failed;
        FailureReasonCode = reasonCode;
        FailureReasonDescription = reasonDescription;
        FailedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
