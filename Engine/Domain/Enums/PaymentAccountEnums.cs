namespace Engine.Domain.Enums;

public enum PaymentAccountType
{
    Individual, // "pf"
    Business // "pj"
}

public enum PaymentAccountStatus
{
    Active,
    Inactive,
    Suspended,
    Closed
}
