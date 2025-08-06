using Engine.Domain.Entities;

namespace Engine.Application.Interfaces.Repositories;

public interface IPaymentMethodRepository : IRepository<PaymentMethod>
{
    public Task<IEnumerable<PaymentMethod>> GetByAccountIdAsync(Guid accountId);
}
