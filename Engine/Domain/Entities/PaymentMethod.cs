using Engine.Domain.Enums;
using Engine.Domain.ValueObjects;
using System.Text.Json;

namespace Engine.Domain.Entities;

public class PaymentMethod
{
    public Guid Id { get; private set; }
    public Guid AccountId { get; private set; }
    public PaymentMethodType MethodType { get; private set; }
    public string MethodDetails { get; private set; }
    public PaymentMethodStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public PaymentMethod()
    {
        MethodDetails = string.Empty;
    }

    private static PaymentMethod Instantiate(Guid accountId, string jsonDetails, PaymentMethodType type)
    {
        return new PaymentMethod
        {
            Id = Guid.NewGuid(),
            AccountId = accountId,
            MethodType = type,
            MethodDetails = jsonDetails,
            Status = PaymentMethodStatus.Active,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static PaymentMethod CreatePix(Guid accountId, PixDetails details)
    {
        string jsonDetails = JsonSerializer.Serialize(details);

        return Instantiate(accountId, jsonDetails, PaymentMethodType.Pix);
    }

    public static PaymentMethod CreateBankTransfer(Guid accountId, BankDetails details)
    {
        string jsonDetails = JsonSerializer.Serialize(details);

        return Instantiate(accountId, jsonDetails, PaymentMethodType.BankTransfer);
    }
}
