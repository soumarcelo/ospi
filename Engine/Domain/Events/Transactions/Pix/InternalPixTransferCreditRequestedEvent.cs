using Engine.Domain.Interfaces;
using Engine.Domain.ValueObjects;

namespace Engine.Domain.Events.Transactions.Pix;

public record InternalPixTransferCreditRequestedEvent(
    Guid OutgoingTransactionId,
    Guid IncomingTransactionId,
    Guid InitiatorUserId,
    Guid FromAccountId,
    Guid ToAccountId,
    Money Amount,
    EndToEndId E2eId,
    string Description) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
