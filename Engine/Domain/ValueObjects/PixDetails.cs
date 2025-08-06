using Engine.Domain.Enums;

namespace Engine.Domain.ValueObjects;

public sealed class PixDetails
{
    public PixKeyType KeyType { get; private set; }
    public string Key { get; private set; }

    private PixDetails(PixKeyType keyType, string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be null or empty.", nameof(key));
        
        ValidateKey(keyType, key);

        KeyType = keyType;
        Key = key;
    }

    public static PixDetails CreateWithCpf(string cpf)
    {
        ValidateKey(PixKeyType.CPF, cpf);
        return new(PixKeyType.CPF, cpf);
    }

    public static PixDetails CreateWithCnpj(string cnpj)
    {
        ValidateKey(PixKeyType.CNPJ, cnpj);
        return new(PixKeyType.CNPJ, cnpj);
    }

    public static PixDetails CreateWithEmail(string email)
    {
        ValidateKey(PixKeyType.Email, email);
        return new(PixKeyType.Email, email);
    }

    public static PixDetails CreateWithPhoneNumber(string phoneNumber)
    {
        ValidateKey(PixKeyType.PhoneNumber, phoneNumber);
        return new(PixKeyType.PhoneNumber, phoneNumber);
    }

    public static PixDetails CreateWithRandomKey(string randomKey)
    {
        ValidateKey(PixKeyType.RandomKey, randomKey);
        return new(PixKeyType.RandomKey, randomKey);
    }

    public static void ValidateKey(PixKeyType keyType, string key)
    {
        switch (keyType)
        {
            case PixKeyType.CPF:
                if (!IndividualDocument.IsValidCpf(key))
                    throw new ArgumentException("Invalid CPF document.", nameof(key));
                break;
            case PixKeyType.CNPJ:
                if (!BusinessDocument.IsValidCnpj(key))
                    throw new ArgumentException("Invalid CNPJ document.", nameof(key));
                break;
            case PixKeyType.Email:
                if (!EmailAddress.IsValidEmail(key))
                    throw new ArgumentException("Invalid email address format.", nameof(key));
                break;
            case PixKeyType.PhoneNumber:
                // Validate phone number format here
                break;
            case PixKeyType.RandomKey:
                if (!Guid.TryParse(key, out _))
                    throw new ArgumentException("Invalid random key format. Must be a valid GUID.", nameof(key));
                break;
        }
    }
}
