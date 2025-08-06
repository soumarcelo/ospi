using Engine.Domain.Entities;

namespace Engine.Application.Interfaces.Repositories;

public interface IPaymentAccountRepository : IRepository<PaymentAccount>
{
    //public Task<PaymentAccount?> GetByUserIdAsync(Guid userId);
}
