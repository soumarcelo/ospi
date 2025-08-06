using Engine.Domain.Enums;
using Engine.Domain.ValueObjects;

namespace Engine.Domain.Entities;

public class PaymentAccount
{
    public Guid Id { get; private set; }
    public PaymentAccountType AccountType { get; private set; }
    public string Document { get; private set; }
    public string LegalName { get; private set; }
    public PaymentAccountStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? SuspendedAt { get; private set; }
    public DateTime? ClosedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public PaymentAccount()
    {
        Document = string.Empty;
        LegalName = string.Empty;
    }

    private static PaymentAccount Instantiate(string document, string legalName, PaymentAccountType type)
    {
        if (string.IsNullOrWhiteSpace(legalName))
            throw new ArgumentException("Legal name cannot be null or empty.", nameof(legalName));

        return new PaymentAccount
        {
            Id = Guid.NewGuid(),
            AccountType = type,
            Document = document,
            LegalName = legalName,
            Status = PaymentAccountStatus.Active,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static PaymentAccount CreateIndividual(IndividualDocument document, string legalName)
    {
        return Instantiate(document.ToString(), legalName, PaymentAccountType.Individual);
    }

    public static PaymentAccount CreateBusiness(BusinessDocument document, string legalName)
    {
        return Instantiate(document.ToString(), legalName, PaymentAccountType.Business);
    }

    public bool CanTransact()
    {
        return Status == PaymentAccountStatus.Active;
    }
}
