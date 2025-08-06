using Engine.Domain.Enums;
using Engine.Domain.ValueObjects;
using Engine.Domain.ValueObjects.TransactionDetails;
using System.Text.Json;

namespace Engine.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }

    // Auditoria e rastreabilidade
    public Guid? InitiatorUserId { get; private set; }
    public Guid? ReversalOfTransactionId { get; private set; }

    // Identificação da conta de pagamento de origem e destino
    public Guid? FromAccountId { get; private set; }
    public Guid? ToAccountId { get; private set; }

    // Identificador externo (E2E para PIX, NSU, etc)
    public TransactionExternalReferenceType? ExternalReferenceType { get; private set; }
    public string? ExternalReference { get; private set; }

    // Valor e Taxa
    public Money Amount { get; private set; }
    public Money FeeAmount { get; private set; }

    // Status regulatório
    public TransactionStatus Status { get; private set; }
    public TransactionFailureReasonCode? FailureReasonCode { get; private set; }
    public string? FailureReasonDescription { get; private set; }

    // Transaction Information
    public TransactionDirection Direction { get; private set; }
    public TransactionType TransactionType { get; private set; }

    // Identificação do método de pagamento utilizado (PIX, TED, etc)
    public TransactionMethodType MethodType { get; private set; }
    
    // Indica se a transação é de um sistema externo (ex: PIX externo)
    public bool IsExternal { get; private set; }

    // Detalhes específicos do método (JSON, para extensibilidade)
    public string? TransactionDetails { get; private set; }
    public string? Description { get; private set; }

    // Auditoria
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public DateTime? FailedAt { get; private set; }
    public DateTime? CancelledAt { get; private set; }
    public DateTime? ReversedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public Transaction()
    {
        Amount = Money.Zero;
        FeeAmount = Money.Zero;
    }

    private static Transaction Instantiate(
        Money amount,
        TransactionDirection direction,
        TransactionType transactionType,
        TransactionMethodType methodType,
        Guid? initiatorUserId = null,
        Guid? fromAccountId = null,
        Guid? toAccountId = null,
        TransactionExternalReferenceType? externalReferenceType = null,
        string? externalReference = null,
        bool isExternal = false,
        object? transactionDetailsObject = null, // Usamos 'object' para ser genérico para PixTransactionDetails, BankDetails, etc.
        string? description = null)
    {
        if (amount == Money.Zero)
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

        string? serializedDetails = null;
        if (transactionDetailsObject != null)
        {
            serializedDetails = JsonSerializer.Serialize(transactionDetailsObject);
        }

        return new Transaction
        {
            Id = Guid.NewGuid(),
            InitiatorUserId = initiatorUserId,
            FromAccountId = fromAccountId,
            ToAccountId = toAccountId,
            Amount = amount,
            FeeAmount = Money.Zero,
            Status = TransactionStatus.Pending,
            Direction = direction,
            TransactionType = transactionType,
            MethodType = methodType,
            ExternalReferenceType = externalReferenceType,
            ExternalReference = externalReference,
            IsExternal = isExternal,
            TransactionDetails = serializedDetails,
            Description = description,
            CreatedAt = DateTime.UtcNow,
        };
    }

    public static Transaction CreatePixIncomingFromInternal(
        Guid initiatorUserId,
        Guid fromAccountId,
        Guid toAccountId,
        Money amount,
        TransactionType transactionType,
        EndToEndId endToEndId,
        string? description = null)
    {
        return Instantiate(
            amount: amount,
            direction: TransactionDirection.Incoming,
            transactionType: transactionType,
            methodType: TransactionMethodType.Pix,
            initiatorUserId: initiatorUserId,
            fromAccountId: fromAccountId,
            toAccountId: toAccountId,
            externalReferenceType: TransactionExternalReferenceType.EndToEnd,
            externalReference: endToEndId.Value,
            description: description
        );
    }

    public static Transaction CreatePixIncomingFromExternal(
        Guid toAccountId,
        Money amount,
        PixTransactionDetails details,
        TransactionType transactionType,
        EndToEndId endToEndId,
        string? description = null)
    {
        return Instantiate(
            amount: amount,
            direction: TransactionDirection.Incoming,
            transactionType: transactionType,
            methodType: TransactionMethodType.Pix,
            toAccountId: toAccountId,
            externalReferenceType: TransactionExternalReferenceType.EndToEnd,
            externalReference: endToEndId.Value,
            isExternal: true,
            transactionDetailsObject: details,
            description: description
        );
    }

    public static Transaction CreatePixOutgoingToInternal(
        Guid initiatorUserId,
        Guid fromAccountId,
        Guid toAccountId,
        Money amount,
        TransactionType transactionType,
        EndToEndId endToEndId,
        string? description = null)
    {
        return Instantiate(
            amount: amount,
            direction: TransactionDirection.Outgoing,
            transactionType: transactionType,
            methodType: TransactionMethodType.Pix,
            initiatorUserId: initiatorUserId,
            fromAccountId: fromAccountId,
            toAccountId: toAccountId,
            externalReferenceType: TransactionExternalReferenceType.EndToEnd,
            externalReference: endToEndId.Value,
            description: description
        );
    }

    public static Transaction CreatePixOutgoingToExternal(
        Guid initiatorUserId,
        Guid fromAccountId,
        Money amount,
        PixTransactionDetails details,
        TransactionType transactionType,
        EndToEndId endToEndId,
        string? description = null)
    {
        return Instantiate(
            amount: amount,
            direction: TransactionDirection.Outgoing,
            transactionType: transactionType,
            methodType: TransactionMethodType.Pix,
            initiatorUserId: initiatorUserId,
            fromAccountId: fromAccountId,
            externalReferenceType: TransactionExternalReferenceType.EndToEnd,
            externalReference: endToEndId.Value,
            isExternal: true,
            transactionDetailsObject: details,
            description: description
        );
    }

    public static Transaction CreatePixRefundIncomingFromInternal(
        Guid transactionToRefundId,
        Guid initiatorUserId,
        Guid fromAccountId,
        Guid toAccountId,
        Money amount,
        EndToEndId endToEndId,
        string? description = null)
    {
        Transaction transaction = CreatePixIncomingFromInternal(
            initiatorUserId,
            fromAccountId,
            toAccountId,
            amount,
            TransactionType.Refund,
            endToEndId,
            description);
        transaction.ReversalOfTransactionId = transactionToRefundId;

        return transaction;
    }

    public static Transaction CreatePixRefundIncomingFromExternal(
        Guid transactionToRefundId,
        Guid toAccountId,
        Money amount,
        PixTransactionDetails details,
        EndToEndId endToEndId,
        string? description = null)
    {
        Transaction transaction = CreatePixIncomingFromExternal(
            toAccountId,
            amount,
            details,
            TransactionType.Refund,
            endToEndId,
            description);
        transaction.ReversalOfTransactionId = transactionToRefundId;

        return transaction;
    }

    public static Transaction CreatePixRefundOutgoingToInternal(
        Guid transactionToRefundId,
        Guid initiatorUserId,
        Guid fromAccountId,
        Guid toAccountId,
        Money amount,
        EndToEndId endToEndId,
        string? description = null)
    {
        Transaction transaction = CreatePixOutgoingToInternal(
            initiatorUserId,
            fromAccountId,
            toAccountId,
            amount,
            TransactionType.Refund,
            endToEndId,
            description);
        transaction.ReversalOfTransactionId = transactionToRefundId;

        return transaction;
    }

    public static Transaction CreatePixRefundOutgoingToExternal(
        Guid transactionToRefundId,
        Guid initiatorUserId,
        Guid fromAccountId,
        Money amount,
        EndToEndId endToEndId,
        PixTransactionDetails details,
        string? description = null)
    {
        Transaction transaction = CreatePixOutgoingToExternal(
            initiatorUserId,
            fromAccountId,
            amount,
            details,
            TransactionType.Refund,
            endToEndId,
            description);
        transaction.ReversalOfTransactionId = transactionToRefundId;

        return transaction;
    }

    public void Failure(TransactionFailureReasonCode reasonCode, string reasonDescription)
    {
        if (Status != TransactionStatus.Pending)
            throw new InvalidOperationException("Only pending transactions can be marked as failed.");

        FailureReasonCode = reasonCode;
        FailureReasonDescription = reasonDescription;
        Status = TransactionStatus.Failed;
        FailedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Refund()
    {
        if (Status != TransactionStatus.Pending || Status != TransactionStatus.Completed)
            throw new InvalidOperationException("Only pending or completed transactions can be refunded.");

        Status = TransactionStatus.Reversed;
        ReversedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status != TransactionStatus.Pending)
            throw new InvalidOperationException("Only pending transactions can be cancelled.");

        Status = TransactionStatus.Cancelled;
        CancelledAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        if (Status != TransactionStatus.Pending)
            throw new InvalidOperationException("Only pending transactions can be completed.");

        Status = TransactionStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsPending() 
        => Status == TransactionStatus.Pending;

    public bool IsCompleted()
        => Status == TransactionStatus.Completed;
}
