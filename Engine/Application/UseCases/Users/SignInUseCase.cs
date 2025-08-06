using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
//using Engine.Application.Interfaces;
using Engine.Application.Interfaces.Services;
using Engine.Domain.Entities;
using Engine.Domain.Enums;
using Engine.Domain.ValueObjects;

namespace Engine.Application.UseCases.Users;

public class SignInUseCase(
    //IUnitOfWork unitOfWork,
    IUserService userService,
    IAuthCredentialService authCredentialsService,
    IPasswordService<User> passwordService)
{
    public async Task<IResult<Guid>> Execute(EmailAddress email, string password)
    {
        IResult<User> userResult = await userService.GetUserByEmailAsync(email);
        if (!userResult.TryGetValue(out User user, out Error? error))
        {
            return Result.Failure<Guid>($"User not found: {error?.Message}");
        }

        IResult<AuthCredential> credentialResult =
            await authCredentialsService.GetCredentialByUserIdAsync(user.Id, AuthCredentialProvider.Email);
        if (!credentialResult.TryGetValue(out AuthCredential credential, out error))
        {
            return Result.Failure<Guid>($"Credential not found: {error?.Message}");
        }

        IResult<bool> passwordCheckResult =
            passwordService.VerifyPassword(user, password, credential.SecretHash);
        if (!passwordCheckResult.TryGetValue(out bool isPasswordValid, out _) || !isPasswordValid)
        {
            return Result.Failure<Guid>("Invalid password.");
        }

        // Needs to returns a JWT or session token instead of user ID in a real-world scenario
        return Result.Success(user.Id);
        
        //await unitOfWork.BeginTransactionAsync();
        
        //try
        //{
        //    await unitOfWork.CommitTransactionAsync();
        //    return Result.Success(user.Id);
        //}
        //catch (Exception ex)
        //{
        //    await unitOfWork.RollbackTransactionAsync();
        //    return Result.Failure<Guid>($"Error during sign-in: {ex.Message}");
        //}
    }
}
