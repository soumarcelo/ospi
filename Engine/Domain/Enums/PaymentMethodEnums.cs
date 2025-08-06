namespace Engine.Domain.Enums;

public enum PaymentMethodType
{
    Pix,
    CreditCard,
    BankTransfer,
}

public enum PaymentMethodStatus
{
    Active,
    Blocked,
    Inactive,
}

public enum PixKeyType
{
    CPF, // Cadastro de Pessoas Físicas (Brazilian Individual Taxpayer Registry)
    CNPJ, // Cadastro Nacional da Pessoa Jurídica (Brazilian National Registry of Legal Entities)
    Email,
    PhoneNumber,
    RandomKey, // A randomly generated key
}
