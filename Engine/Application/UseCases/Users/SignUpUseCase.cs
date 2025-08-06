using Engine.Application.Interfaces.Services;
using Engine.Application.Interfaces;
using Engine.Application.Common.Results;
using Engine.Application.Requests;
using Engine.Domain.Entities;
using Engine.Application.Common.Errors;

namespace Engine.Application.UseCases.Users;

public class SignUpUseCase(
    IUnitOfWork unitOfWork,
    IUserService userService,
    IPasswordService<User> passwordService,
    IAuthCredentialService authCredentialsService)
{
    public async Task<IResult<Guid>> Execute(SignUpRequest request)
    {
        await unitOfWork.BeginTransactionAsync();

        CreateUserRequest userRequest = new(
            request.FullName,
            request.Email,
            request.Document);

        IResult<User> createUserResult = await userService.AddUserAsync(userRequest);
        if (!createUserResult.TryGetValue(out User user, out Error? error))
        {
            await unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Guid>($"Failed to create user: {error?.Message}");
        }

        IResult<string> passwordHashResult =
            passwordService.HashPassword(user, request.Password);
        if (!passwordHashResult.TryGetValue(out string hashedPassword, out error))
        {
            await unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Guid>($"Failed to hash password: {error?.Message} or password is empty.");
        }

        AuthCredentialRequest credentialRequest = new(
            user.Id,
            request.Email,
            hashedPassword);

        IResult<AuthCredential> resultCreate = 
            await authCredentialsService.CreateEmailCredentialAsync(credentialRequest);

        if (!resultCreate.TryGetValue(out AuthCredential _, out error))
        {
            await unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Guid>($"Failed to create email credential: {error?.Message}");
        }

        try
        {
            await unitOfWork.CommitTransactionAsync();
            return Result.Success(user.Id);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Guid>($"Failed to commit transaction: {ex.Message}");
        }
    }
}
