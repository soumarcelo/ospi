using Engine.Domain.ValueObjects;

namespace Engine.Application.Requests;

public record CreateUserRequest(
    string FullName,
    EmailAddress ContactEmail,
    IndividualDocument Document);
