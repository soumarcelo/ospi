using System.Text.RegularExpressions;

namespace Engine.Domain.ValueObjects;

public partial class EmailAddress
{
    public string Value { get; private set; }

    public EmailAddress(string email)
    {
        if (!IsValidEmail(email))
        {
            throw new ArgumentException("Invalid email address format.", nameof(email));
        }

        Value = email;
    }

    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled)]
    private static partial Regex EmailRegex();

    public static bool IsValidEmail(string email)
    {
        return EmailRegex().IsMatch(email);
    }

    public override string ToString() => Value;

    public static EmailAddress Parse(string email) => new(email);

    public static EmailAddress Invalid => new("contact@email.com");
}
