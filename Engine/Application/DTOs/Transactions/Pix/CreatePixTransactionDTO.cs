using Engine.Domain.ValueObjects;

namespace Engine.Application.DTOs.Transactions.Pix;

public class CreatePixTransactionDTO
{
    public Guid InitiatorUserId { get; set; }
    public Guid FromAccountId { get; set; }
    public Guid ToAccountId { get; set; }
    public Money Amount { get; set; } = Money.Zero;
    public string Description { get; set; } = string.Empty;
    //public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
}
