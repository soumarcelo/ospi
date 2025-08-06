using Engine.Domain.Entities;

namespace Engine.Application.Interfaces.Repositories;

public interface IPaymentAccountUserRepository : IRepository<PaymentAccountUser>
{
    public Task<IEnumerable<PaymentAccountUser>> GetByAccountIdAsync(Guid accountId);
    public Task<IEnumerable<PaymentAccountUser>> GetByUserIdAsync(Guid userId);
    public Task<bool> CanUserTransactWithAccount(Guid userId, Guid accountId);
}
