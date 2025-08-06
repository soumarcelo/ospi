using Engine.Domain.Enums;
using Engine.Domain.ValueObjects;

namespace Engine.Domain.Entities;

public class AuthCredential
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public AuthCredentialProvider AuthProvider { get; private set; }
    public string ProviderKey { get; private set; }
    public string SecretHash { get; private set; }
    public bool IsVerified { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? VerifiedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public AuthCredential()
    {
        ProviderKey = string.Empty;
        SecretHash = string.Empty;
    }

    public static AuthCredential CreateWithEmail(Guid userId, EmailAddress email, string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword))
            throw new ArgumentException("Hashed password cannot be null or empty.", nameof(hashedPassword));

        return new AuthCredential
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            AuthProvider = AuthCredentialProvider.Email,
            ProviderKey = email.ToString(),
            SecretHash = hashedPassword,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Verify()
    {
        IsVerified = true;
        VerifiedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
