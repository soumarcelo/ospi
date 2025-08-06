namespace Engine.Domain.ValueObjects.TransactionDetails;

public record PixTransactionDetails(
    PixDetails PixInfo, 
    TransactionCounterparty Counterparty);
