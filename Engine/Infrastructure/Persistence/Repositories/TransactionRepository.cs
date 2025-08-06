using Engine.Application.Interfaces.Repositories;
using Engine.Domain.Entities;
using Engine.Domain.Enums;
using Engine.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Engine.Infrastructure.Persistence.Repositories;

public class TransactionRepository(AppDbContext _context) : ITransactionRepository
{
    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        return await _context.Transactions.FindAsync(id);
    }

    public async Task<Transaction?> GetByEndToEndIdAsync(EndToEndId endToEndId)
    {
        return await _context.Transactions
            .FirstOrDefaultAsync(t =>
                t.ExternalReferenceType == TransactionExternalReferenceType.EndToEnd &&
                t.ExternalReference == endToEndId.Value);
    }

    public async Task<IList<Transaction>> GetTransactionsByAccountIdAsync(Guid accountId)
    {
        return await _context.Transactions.Select(t => t)
            .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IList<Transaction>> GetStatementTransactionsByAccountIdAsync(Guid accountId)
    {
        return await _context.Transactions
            .Where(t => 
                (t.FromAccountId == accountId || t.ToAccountId == accountId) &&
                t.Status != TransactionStatus.Pending)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<decimal> GetBalanceByAccountIdAsync(Guid accountId)
    {
        decimal credits = await _context.Transactions
            .Where(t => t.ToAccountId == accountId && 
            t.Status == TransactionStatus.Completed &&
            t.Direction == TransactionDirection.Incoming)
            .SumAsync(t => t.Amount.Value);
        decimal debits = await _context.Transactions
            .Where(t => t.FromAccountId == accountId && 
            t.Status == TransactionStatus.Completed &&
            t.Direction == TransactionDirection.Outgoing)
            .SumAsync(t => t.Amount.Value);
        return credits - debits;
    }

    public async Task AddAsync(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
    }

    public void Update(Transaction transaction)
    {
        _context.Transactions.Update(transaction);
    }

    public async Task DeleteAsync(Guid transactionId)
    {
        Transaction? transaction = await GetByIdAsync(transactionId) ??
            throw new KeyNotFoundException($"Transaction with ID {transactionId} not found.");
        _context.Transactions.Remove(transaction);
    }
}
