using Engine.Domain.Entities;
using Engine.Domain.ValueObjects;

namespace Engine.Application.Interfaces.Repositories;

public interface ITransactionRepository : IRepository<Transaction>
{
    public Task<Transaction?> GetByEndToEndIdAsync(EndToEndId endToEndId);
    public Task<IList<Transaction>> GetTransactionsByAccountIdAsync(Guid accountId);
    public Task<IList<Transaction>> GetStatementTransactionsByAccountIdAsync(Guid accountId);
    public Task<decimal> GetBalanceByAccountIdAsync(Guid accountId);
}
