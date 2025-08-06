using Engine.Domain.Entities;
using Engine.Domain.ValueObjects;

namespace Engine.Application.Requests.Transactions;

public record PixIncomingTransactionRequest(
    User InitiatorUser,
    PaymentAccount FromAccount,
    PaymentAccount ToAccount,
    Money Amount,
    EndToEndId E2EId,
    string Description);
