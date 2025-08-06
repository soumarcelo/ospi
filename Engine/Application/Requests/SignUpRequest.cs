using Engine.Application.DTOs.Users;
using Engine.Domain.ValueObjects;

namespace Engine.Application.Requests;

public record SignUpRequest(
    string FullName,
    string Password,
    EmailAddress Email,
    IndividualDocument Document)
{
    public static SignUpRequest FromDTO(SignUpDTO dto)
    {
        return new SignUpRequest(
            dto.FullName,
            dto.Password,
            EmailAddress.Parse(dto.Email),
            IndividualDocument.Parse(dto.Document));
    }
}
