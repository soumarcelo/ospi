using System.Text.RegularExpressions;

namespace Engine.Domain.ValueObjects;

public partial class IndividualDocument
{
    public string Value { get; }

    public IndividualDocument(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("CPF can't be empty or null.", nameof(cpf));

        string digits = OnlyDigits(cpf);

        if (!IsValidCpf(digits))
            throw new ArgumentException("Invalid CPF number.", nameof(cpf));

        Value = digits;
    }

    public override string ToString() => Value;

    public static IndividualDocument Parse(string cpf) => new(cpf);

    public static IndividualDocument Invalid => new("049.285.178-65");

    /// <summary>
    /// Retorna o CPF mascarado conforme recomendação do BACEN: ***.XXX.***-**
    /// Exemplo: 12345678901 => ***.456.***-**
    /// </summary>
    public string ToPublicString()
    {
        if (Value.Length != 11)
            return string.Empty;

        string part = Value.Substring(3, 3); 
        return $"***.{part}.***-**";
    }

    public override bool Equals(object? obj)
        => obj is IndividualDocument other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    private static string OnlyDigits(string input)
        => new([.. input.Where(char.IsDigit)]);

    public static bool IsValidCpf(string cpf)
    {
        if (!CPFRegex().IsMatch(cpf))
            return false;

        // Rejeita CPFs com todos os dígitos iguais
        if (cpf.Distinct().Count() == 1)
            return false;

        // Validação dos dígitos verificadores
        int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf[..9];
        int soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        tempCpf += digito1;
        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        return cpf.EndsWith($"{digito1}{digito2}");
    }

    [GeneratedRegex(@"^\d{11}$", RegexOptions.Compiled)]
    private static partial Regex CPFRegex();
}
