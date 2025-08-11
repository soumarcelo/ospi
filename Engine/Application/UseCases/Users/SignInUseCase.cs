using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
using Engine.Application.DTOs.AuthCredentials;
using Engine.Application.Interfaces;
using Engine.Application.Interfaces.Services;
using Engine.Application.Requests;
using Engine.Domain.Entities;
using Engine.Domain.Enums;

namespace Engine.Application.UseCases.Users;

public class SignInUseCase(
    //IUnitOfWork unitOfWork,
    IUserService userService,
    IAuthCredentialService authCredentialsService,
    IPasswordService<User> passwordService,
    ITokenService tokenService)
{
    public async Task<IResult<AuthTokenDTO>> Execute(SignInRequest request)
    {
        IResult<User> userResult = await userService.GetUserByEmailAsync(request.Email);
        if (!userResult.TryGetValue(out User user, out Error? error))
        {
            return Result.Failure<AuthTokenDTO>($"User not found: {error?.Message}");
        }

        IResult<AuthCredential> credentialResult =
            await authCredentialsService.GetCredentialByUserIdAsync(user.Id, AuthCredentialProvider.Email);
        if (!credentialResult.TryGetValue(out AuthCredential credential, out error))
        {
            return Result.Failure<AuthTokenDTO>($"Credential not found: {error?.Message}");
        }

        IResult<bool> passwordCheckResult =
            passwordService.VerifyPassword(user, request.Password, credential.SecretHash);
        if (!passwordCheckResult.TryGetValue(out bool isPasswordValid, out _) || !isPasswordValid)
        {
            return Result.Failure<AuthTokenDTO>("Invalid password.");
        }

        AuthTokenDTO token = await tokenService.GenerateToken(user, credential);
        return Result.Success(token);
        
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
