using Engine.Domain.Entities;
using Engine.Domain.ValueObjects;

namespace Engine.Application.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>
{
    public Task<User?> GetByEmailAsync(EmailAddress email);
    public Task<User?> GetByDocumentAsync(IndividualDocument document);
}
