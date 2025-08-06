using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
using Engine.Application.DTOs.AuthCredentials;
using Engine.Application.Interfaces;
using Engine.Application.Interfaces.Services;
using Engine.Application.Requests;
using Engine.Domain.Entities;

namespace Engine.Application.UseCases.Credentials;

public class CreateEmailCredentialToUserUseCase(
    IUnitOfWork unitOfWork,
    IAuthCredentialService authCredentialsService)
{
    public async Task<IResult<Guid>> Execute(Guid userId, CreateEmailCredentialDTO dto)
    {
        AuthCredentialRequest credentialRequest = new(
                userId,
                dto.Email,
                dto.HashedPassword);

        IResult<AuthCredential> resultCreate = 
            await authCredentialsService.CreateEmailCredentialAsync(credentialRequest);

        if (!resultCreate.TryGetValue(out AuthCredential credential, out Error? error))
        {
            await unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Guid>($"Failed to create email credential: {error?.Message}");
        }
        
        await unitOfWork.BeginTransactionAsync();

        IResult<Guid> resultAdd = await authCredentialsService.AddCredentialAsync(credential);
        if (!resultAdd.TryGetValue(out Guid _, out error))
        {
            return Result.Failure<Guid>($"Failed to add email credential: {error?.Message}");
        }

        try
        {
            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitTransactionAsync();

            return Result.Success(credential.Id);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Guid>($"An error occurred while creating the email credential: {ex.Message}");
        }
    }
}