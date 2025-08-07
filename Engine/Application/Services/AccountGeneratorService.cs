using Engine.Application.Interfaces;

namespace Engine.Application.Services;

public class AccountGeneratorService : IAccountGeneratorService
{
    private const int AccountNumberLength = 5;

    public string GenerateAccountNumber()
    {
        // Generate a random account number with a specific prefix and length
        Random random = new();
        string accountNumber = string.Empty;
        for (int i = 0; i <= AccountNumberLength; i++)
        {
            accountNumber += random.Next(0, 10).ToString(); // Append random digits
        }
        return accountNumber;
    }

    public string CalculateCheckDigit(string accountNumber)
    {
        // Calculate the check digit using the Luhn algorithm
        int sum = 0;
        bool alternate = false;
        for (int i = accountNumber.Length - 1; i >= 0; i--)
        {
            int n = int.Parse(accountNumber[i].ToString());
            if (alternate)
            {
                n *= 2;
                if (n > 9)
                {
                    n -= 9; // Subtract 9 if the result is greater than 9
                }
            }
            sum += n;
            alternate = !alternate; // Toggle the alternate flag
        }
        int checkDigit = (10 - (sum % 10)) % 10; // Calculate the check digit
        return checkDigit.ToString();
    }
}
