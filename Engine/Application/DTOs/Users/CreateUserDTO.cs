using Engine.Domain.ValueObjects;

namespace Engine.Application.DTOs.Users;

public class CreateUserDTO
{
    public string FullName { get; set; } = string.Empty;
    public EmailAddress ContactEmail { get; set; } = EmailAddress.Invalid;
    public IndividualDocument Document { get; set; } = IndividualDocument.Invalid;
}
