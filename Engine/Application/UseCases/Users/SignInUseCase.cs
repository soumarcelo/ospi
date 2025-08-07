using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
using Engine.Application.DTOs.Users;

//using Engine.Application.Interfaces;
using Engine.Application.Interfaces.Services;
using Engine.Application.Requests;
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
    public async Task<IResult<SignInResponseDTO>> Execute(SignInRequest request)
    {
        IResult<User> userResult = await userService.GetUserByEmailAsync(request.Email);
        if (!userResult.TryGetValue(out User user, out Error? error))
        {
            return Result.Failure<SignInResponseDTO>($"User not found: {error?.Message}");
        }

        IResult<AuthCredential> credentialResult =
            await authCredentialsService.GetCredentialByUserIdAsync(user.Id, AuthCredentialProvider.Email);
        if (!credentialResult.TryGetValue(out AuthCredential credential, out error))
        {
            return Result.Failure<SignInResponseDTO>($"Credential not found: {error?.Message}");
        }

        IResult<bool> passwordCheckResult =
            passwordService.VerifyPassword(user, request.Password, credential.SecretHash);
        if (!passwordCheckResult.TryGetValue(out bool isPasswordValid, out _) || !isPasswordValid)
        {
            return Result.Failure<SignInResponseDTO>("Invalid password.");
        }

        SignInResponseDTO response = new()
        {
            Token = string.Empty, // Placeholder for JWT or session token
            RefreshToken = string.Empty, // Placeholder for refresh token
            ExpiresAt = DateTime.UtcNow.AddHours(1), // Example expiration time
            RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(30) // Example refresh token expiration time
        };

        // Needs to returns a JWT or session token instead of user ID in a real-world scenario
        return Result.Success(response);
        
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
