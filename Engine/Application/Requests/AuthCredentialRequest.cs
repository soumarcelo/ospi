using Engine.Domain.ValueObjects;

namespace Engine.Application.Requests;

public record AuthCredentialRequest(
    Guid UserId,
    EmailAddress Email,
    string HashedPassword);