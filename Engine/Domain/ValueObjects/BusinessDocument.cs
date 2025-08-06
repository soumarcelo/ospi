using System.Text.RegularExpressions;

namespace Engine.Domain.ValueObjects;

public partial class BusinessDocument
{
    public string Value { get; }

    public BusinessDocument(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            throw new ArgumentException("CNPJ can't be empty or null.", nameof(cnpj));

        string digits = OnlyDigits(cnpj);

        if (!IsValidCnpj(digits))
            throw new ArgumentException("Invalid CNPJ number.", nameof(cnpj));

        Value = digits;
    }

    public override string ToString() => Value;

    public static BusinessDocument Parse(string cnpj) => new(cnpj);

    public static BusinessDocument Invalid => new("84.683.123/0001-64");


    /// <summary>
    /// Retorna o CNPJ mascarado: ***.***.***/XXXX-YY
    /// Exemplo: 12345678000195 => ***.***.***/0001-12
    /// </summary>
    public string ToPublicString()
    {
        if (Value.Length != 14)
            return string.Empty;

        string branch = Value.Substring(8, 4);
        string digit = Value.Substring(12, 2);
        return $"***.***.***/{branch}-{digit}";
    }

    public override bool Equals(object? obj)
        => obj is BusinessDocument other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    private static string OnlyDigits(string input)
        => new([.. input.Where(char.IsDigit)]);

    public static bool IsValidCnpj(string cnpj)
    {
        if (!CnpjRegex().IsMatch(cnpj))
            return false;

        // rejeita sequências iguais
        if (cnpj.Distinct().Count() == 1)
            return false;

        int[] mult1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] mult2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        string temp = cnpj.Substring(0, 12);
        int sum = temp
            .Select((c, i) => (c - '0') * mult1[i])
            .Sum();

        int rem = sum % 11;
        int dig1 = rem < 2 ? 0 : 11 - rem;

        temp += dig1;
        sum = temp
            .Select((c, i) => (c - '0') * mult2[i])
            .Sum();

        rem = sum % 11;
        int dig2 = rem < 2 ? 0 : 11 - rem;

        return cnpj.EndsWith($"{dig1}{dig2}");
    }

    [GeneratedRegex(@"^\d{14}$", RegexOptions.Compiled)]
    private static partial Regex CnpjRegex();
}