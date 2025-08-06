using Engine.Domain.ValueObjects;

namespace Engine.Application.DTOs.PaymentAccounts;

public class CreateBusinessPaymentAccountDTO
{
    public string LegalName { get; set; } = string.Empty;
    public BusinessDocument Document { get; set; } = BusinessDocument.Invalid;
}
