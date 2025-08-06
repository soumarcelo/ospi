using Engine.Domain.Enums;

namespace Engine.Domain.Entities;

public class PaymentAccountUser
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public Guid UserId { get; private set; }
    public PaymentAccountUserRole Role { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public PaymentAccountUser() { }

    public static PaymentAccountUser Create(
        Guid accountId, 
        Guid userId, 
        PaymentAccountUserRole role = PaymentAccountUserRole.Owner)
    {
        return new PaymentAccountUser
        {
            Id = Guid.NewGuid(),
            AccountId = accountId,
            UserId = userId,
            Role = role,
            CreatedAt = DateTime.UtcNow
        };
    }
}
