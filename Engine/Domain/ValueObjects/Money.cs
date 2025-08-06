namespace Engine.Domain.ValueObjects;

public record Money
{
    public decimal Value { get; private set; }
    public string Currency { get; private set; } = "BRL";

    // Construtor principal para garantir que o valor não seja negativo, se essa for sua regra.
    public Money(decimal value, string currency)
    {
        if (value < 0)
            throw new ArgumentException("Money value cannot be negative.", nameof(value));

        Value = value;
        Currency = currency;
    }

    // Construtor auxiliar para valores BRL, passando pelo construtor principal para validação.
    public Money(decimal amount) : this(amount, "BRL") { }

    public static Money Zero => new(0);
    public static Money One => new(1);

    public bool IsValid => this > Zero;

    public override string ToString() => $"{Value:F2} {Currency}";

    // --- Sobrecarga de Operadores Aritméticos ---

    // Validação de Moeda Comum: Essencial para soma, subtração e comparações.
    private static void EnsureSameCurrency(Money a, Money b)
    {
        if (a.Currency != b.Currency)
        {
            throw new InvalidOperationException($"Cannot perform operation on Money with different currencies: {a.Currency} vs {b.Currency}.");
        }
    }

    // Soma (+)
    public static Money operator +(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return new(a.Value + b.Value, a.Currency);
    }

    // Subtração (-)
    public static Money operator -(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        // Opcional: Adicionar validação se a subtração não deve resultar em valor negativo.
        return new(a.Value - b.Value, a.Currency);
    }

    // Multiplicação (*) por um fator decimal: Retorna decimal para preservar precisão.
    public static decimal operator *(Money money, decimal multiplier)
    {
        return money.Value * multiplier;
    }

    public static decimal operator *(decimal multiplier, Money money)
    {
        return money * multiplier;
    }

    // Divisão (/) por um divisor decimal: Retorna decimal para preservar precisão.
    public static decimal operator /(Money money, decimal divisor)
    {
        if (divisor == 0)
            throw new DivideByZeroException("Cannot divide by zero.");
        return money.Value / divisor;
    }

    // Divisão (/) por outro Money: Retorna decimal (razão ou proporção).
    public static decimal operator /(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        if (b.Value == 0)
            throw new DivideByZeroException("Cannot divide by zero by Money.Zero.");
        return a.Value / b.Value;
    }

    // --- Sobrecarga de Operadores de Comparação ---

    // Menor que (<)
    public static bool operator <(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return a.Value < b.Value;
    }

    // Maior que (>)
    public static bool operator >(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return a.Value > b.Value;
    }

    // Menor ou Igual a (<=)
    public static bool operator <=(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return a.Value <= b.Value;
    }

    // Maior ou Igual a (>=)
    public static bool operator >=(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return a.Value >= b.Value;
    }
}
