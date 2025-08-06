using Engine.Application.Common.Results;
using Engine.Application.Interfaces.Repositories;
using Engine.Application.Interfaces.Services;
using Engine.Application.Requests;
using Engine.Domain.Entities;
using Engine.Domain.Enums;
using Engine.Domain.ValueObjects;
namespace Engine.Application.Services;

public class AuthCredentialService(IAuthCredentialRepository repository) : IAuthCredentialService
{
    public async Task<IResult<AuthCredential>> CreateEmailCredentialAsync(AuthCredentialRequest request)
    {
        AuthCredential credential = 
            AuthCredential.CreateWithEmail(request.UserId, request.Email, request.HashedPassword);
        await AddCredentialAsync(credential);

        return Result.Success(credential);
    }

    public async Task<IResult<Guid>> AddCredentialAsync(AuthCredential credential)
    {
        await repository.AddAsync(credential);
        return Result.Success(credential.Id);
    }

    public async Task<IResult<AuthCredential>> GetCredentialByIdAsync(Guid credentialId)
    {
        AuthCredential? credential = await repository.GetByIdAsync(credentialId);
        if (credential == null)
        {
            return Result.Failure<AuthCredential>("Credential not found");
        }
        return Result.Success(credential);
    }

    public async Task<IResult<AuthCredential>> GetCredentialByEmailAsync(EmailAddress email)
    {
        AuthCredential? credential = await repository.GetByEmailAsync(email);
        if (credential == null)
        {
            return Result.Failure<AuthCredential>("Credential not found for the specified email");
        }
        return Result.Success(credential);
    }

    public async Task<IResult<AuthCredential>> GetCredentialByUserIdAsync(Guid userId, AuthCredentialProvider provider)
    {
        AuthCredential? credential = await repository.GetByUserIdAsync(userId, provider);
        if (credential == null || credential.AuthProvider != provider)
        {
            return Result.Failure<AuthCredential>("Credential not found for the specified user and provider");
        }
        return Result.Success(credential);
    }
    public IResult<Guid> UpdateCredential(AuthCredential credential)
    {
        repository.Update(credential);
        return Result.Success(credential.Id);
    }
}
