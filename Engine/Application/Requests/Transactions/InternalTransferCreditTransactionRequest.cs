using Engine.Domain.Entities;
using Engine.Domain.ValueObjects;

namespace Engine.Application.Requests.Transactions;

public record InternalTransferCreditTransactionRequest(
    Transaction OutgoingTransaction,
    User InitiatorUser,
    PaymentAccount FromAccount,
    PaymentAccount ToAccount,
    Money Amount,
    EndToEndId E2EId,
    string Description = "");
