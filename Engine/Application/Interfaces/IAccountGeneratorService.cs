namespace Engine.Application.Interfaces;

public interface IAccountGeneratorService
{
    public string GenerateAccountNumber();
    public string CalculateCheckDigit(string accountNumber);
}
