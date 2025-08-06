using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
using Engine.Application.Interfaces;
using Engine.Application.Interfaces.Services;
using Engine.Domain.Entities;
using Engine.Domain.ValueObjects;

namespace Engine.Application.UseCases.Credentials;

public class VerifyEmailCredentialUseCase(
    IUnitOfWork unitOfWork,
    IAuthCredentialService authCredentialsService)
{
    public async Task<IResult<AuthCredential?>> Execute(EmailAddress email)
    {
        IResult<AuthCredential> getResult = await authCredentialsService.GetCredentialByEmailAsync(email);

        if (!getResult.TryGetValue(out AuthCredential credential, out Error? error))
        {
            return Result.Failure<AuthCredential?>($"Failed to retrieve credential: {error?.Message}");
        }

        if (credential.IsVerified)
        {
            return Result.Success<AuthCredential?>(credential);
        }
        
        await unitOfWork.BeginTransactionAsync();

        credential.Verify();

        IResult<Guid> updateResult = authCredentialsService.UpdateCredential(credential);

        if (!updateResult.TryGetValue(out Guid _, out error))
        {
            await unitOfWork.RollbackTransactionAsync();
            return Result.Failure<AuthCredential?>($"Failed to update credential: {error?.Message}");
        }

        try
        {
            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitTransactionAsync();

            return Result.Success<AuthCredential?>(credential);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            return Result.Failure<AuthCredential?>($"An error occurred while verifying the email credential: {ex.Message}");
        }
    }
}