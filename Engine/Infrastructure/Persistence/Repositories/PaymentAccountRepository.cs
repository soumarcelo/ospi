using Engine.Application.Interfaces.Repositories;
using Engine.Domain.Entities;

namespace Engine.Infrastructure.Persistence.Repositories;

public class PaymentAccountRepository(AppDbContext _context) : IPaymentAccountRepository
{
    public async Task<PaymentAccount?> GetByIdAsync(Guid id)
    {
        return await _context.PaymentAccounts.FindAsync(id);
    }

    public async Task AddAsync(PaymentAccount paymentAccount)
    {
        await _context.PaymentAccounts.AddAsync(paymentAccount);
    }

    public void Update(PaymentAccount paymentAccount)
    {
        _context.PaymentAccounts.Update(paymentAccount);
    }

    public async Task DeleteAsync(Guid paymentAccountId)
    {
        PaymentAccount? paymentAccount = await GetByIdAsync(paymentAccountId) ??
            throw new KeyNotFoundException($"Payment account with ID {paymentAccountId} not found.");
        _context.PaymentAccounts.Remove(paymentAccount);
    }
}
