using Engine.Application.Interfaces.Repositories;
using Engine.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Engine.Infrastructure.Persistence.Repositories;

public class PaymentMethodRepository(AppDbContext _context) : IPaymentMethodRepository
{
    public async Task<PaymentMethod?> GetByIdAsync(Guid id)
    {
        return await _context.PaymentMethods.FindAsync(id);
    }

    public async Task<IEnumerable<PaymentMethod>> GetByAccountIdAsync(Guid accountId)
    {
        return await _context.PaymentMethods
            .Where(pm => pm.AccountId == accountId)
            .ToListAsync();
    }

    public async Task AddAsync(PaymentMethod paymentMethod)
    {
        await _context.PaymentMethods.AddAsync(paymentMethod);
    }

    public void Update(PaymentMethod paymentMethod)
    {
        _context.PaymentMethods.Update(paymentMethod);
    }

    public async Task DeleteAsync(Guid paymentMethodId)
    {
        PaymentMethod? paymentMethod = await GetByIdAsync(paymentMethodId) ??
            throw new KeyNotFoundException($"Payment method with ID {paymentMethodId} not found.");
        _context.PaymentMethods.Remove(paymentMethod);
    }
}
