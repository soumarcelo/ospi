using Engine.Application.Interfaces.Repositories;
using Engine.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Engine.Infrastructure.Persistence.Repositories;

public class PaymentAccountUserRepository(AppDbContext _context) : IPaymentAccountUserRepository
{
    public async Task<PaymentAccountUser?> GetByIdAsync(Guid id)
    {
        return await _context.UserAccounts.FindAsync(id);
    }

    public async Task<IEnumerable<PaymentAccountUser>> GetByAccountIdAsync(Guid accountId)
    {
        return await _context.UserAccounts
            .Where(pau => pau.AccountId == accountId)
            .ToListAsync();
    }

    public async Task<IEnumerable<PaymentAccountUser>> GetByUserIdAsync(Guid userId)
    {
        return await _context.UserAccounts
            .Where(pau => pau.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> CanUserTransactWithAccount(Guid userId, Guid accountId)
    {
        return await _context.UserAccounts
            .AnyAsync(pau => pau.UserId == userId && pau.AccountId == accountId);
    }

    public async Task AddAsync(PaymentAccountUser paymentAccountUser)
    {
        await _context.UserAccounts.AddAsync(paymentAccountUser);
    }

    public void Update(PaymentAccountUser paymentAccountUser)
    {
        _context.UserAccounts.Update(paymentAccountUser);
    }

    public async Task DeleteAsync(Guid paymentAccountUserId)
    {
        PaymentAccountUser? paymentAccountUser = 
            await _context.UserAccounts.FindAsync(paymentAccountUserId) ??
            throw new KeyNotFoundException(
                $"Payment account user with ID {paymentAccountUserId} not found.");
        _context.UserAccounts.Remove(paymentAccountUser);
    }
}
