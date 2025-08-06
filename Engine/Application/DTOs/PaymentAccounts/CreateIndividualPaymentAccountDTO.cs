using Engine.Domain.ValueObjects;

namespace Engine.Application.DTOs.PaymentAccounts;

public class CreateIndividualPaymentAccountDTO
{
    public string LegalName { get; set; } = string.Empty;
    public IndividualDocument Document { get; set; } = IndividualDocument.Invalid;
}
