namespace Engine.Domain.ValueObjects;

public record BankDetails(
    string AccountNumber,
    string AccountCheckDigitNumber,
    string BankCode = "XXX",
    string BranchNumber = "0001");
