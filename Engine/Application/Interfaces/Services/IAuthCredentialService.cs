using Engine.Application.Common.Results;
using Engine.Application.Requests;
using Engine.Domain.Entities;
using Engine.Domain.Enums;
using Engine.Domain.ValueObjects;

namespace Engine.Application.Interfaces.Services;

public interface IAuthCredentialService
{
    public Task<IResult<AuthCredential>> CreateEmailCredentialAsync(AuthCredentialRequest request);
    public Task<IResult<Guid>> AddCredentialAsync(AuthCredential credential);
    public Task<IResult<AuthCredential>> GetCredentialByIdAsync(Guid credentialId);
    public Task<IResult<AuthCredential>> GetCredentialByEmailAsync(EmailAddress email);
    public Task<IResult<AuthCredential>> GetCredentialByUserIdAsync(
        Guid userId,
        AuthCredentialProvider provider);
    public IResult<Guid> UpdateCredential(AuthCredential credential);
}
