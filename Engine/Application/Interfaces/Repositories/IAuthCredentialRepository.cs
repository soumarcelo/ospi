using Engine.Domain.Entities;
using Engine.Domain.Enums;
using Engine.Domain.ValueObjects;

namespace Engine.Application.Interfaces.Repositories;

public interface IAuthCredentialRepository : IRepository<AuthCredential>
{
    public Task<AuthCredential?> GetByUserIdAsync(Guid userId, AuthCredentialProvider provider);
    public Task<AuthCredential?> GetByEmailAsync(EmailAddress email);
}
