using Engine.Domain.Entities;

namespace Engine.Application.DTOs.Transactions;

public class StatementTransactionDTO
{
    public Guid Id { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string TransactionType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Direction { get; set; } = string.Empty;

    public static StatementTransactionDTO FromEntity(Transaction transaction)
    {
        return new StatementTransactionDTO
        {
            Id = transaction.Id,
            TransactionDate = transaction.CreatedAt,
            Description = transaction.Description ?? "",
            Amount = transaction.Amount.Value,
            Currency = transaction.Amount.Currency,
            TransactionType = transaction.TransactionType.ToString(),
            Status = transaction.Status.ToString(),
            Direction = transaction.Direction.ToString()
        };
    }
}
