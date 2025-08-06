using Engine.Domain.Interfaces;
using Engine.Domain.ValueObjects;

namespace Engine.Domain.Events.Transactions.Pix;

public record InternalPixTransferDebitRequestedEvent(
    Guid TransactionId,
    Guid InitiatorUserId,
    Guid FromAccountId,
    Guid ToAccountId,
    Money Amount,
    EndToEndId E2EId,
    string Description) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
