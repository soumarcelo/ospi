using Engine.Application.Common.Results;
using Engine.Application.Interfaces.Services;
using Engine.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Engine.Application.Services;

public class PasswordService(IPasswordHasher<User> passwordHasher) : IPasswordService<User>
{
    public IResult<string> HashPassword(User user, string password)
    {
        return Result.Success(
            passwordHasher.HashPassword(user, password));
    }

    public IResult<bool> VerifyPassword(User user, string hashedPassword, string providedPassword)
    {
        PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(
            user,
            hashedPassword,
            providedPassword);

        return Result.Success(result != PasswordVerificationResult.Failed);
    }
}
