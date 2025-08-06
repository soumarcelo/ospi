using Engine.Domain.ValueObjects;

namespace Engine.Application.DTOs.AuthCredentials;

public class CreateEmailCredentialDTO
{
    public EmailAddress Email { get; set; } = EmailAddress.Invalid;
    public string HashedPassword { get; set; } = string.Empty;
}
