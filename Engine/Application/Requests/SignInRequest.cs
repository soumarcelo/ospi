using Engine.Application.DTOs.Users;
using Engine.Domain.ValueObjects;

namespace Engine.Application.Requests;

public record SignInRequest(
    EmailAddress Email,
    string Password)
{
    public static SignInRequest FromDTO(SignInDTO dto)
    {
        EmailAddress email = new(dto.Email);
        return new SignInRequest(email, dto.Password);
    }
}
