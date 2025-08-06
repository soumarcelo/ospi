using Engine.Domain.Entities;
using Engine.Domain.ValueObjects;

namespace Engine.Application.Requests.Transactions;

public record InternalTransferDebitTransactionRequest(
    User InitiatorUser,
    PaymentAccount FromAccount,
    PaymentAccount ToAccount,
    Money Amount,
    string Description = "");
