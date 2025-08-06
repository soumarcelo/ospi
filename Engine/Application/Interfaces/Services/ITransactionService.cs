using Engine.Application.Common.Results;
using Engine.Application.Requests.Transactions;
using Engine.Domain.Entities;

namespace Engine.Application.Interfaces.Services;

public interface ITransactionService
{
    public Task<IResult<Transaction>> AddTransactionAsync(PixOutgoingTransactionRequest request);
    public Task<IResult<Transaction>> AddTransactionAsync(PixIncomingTransactionRequest request);
    public Task<IResult<Transaction>> GetTransactionByIdAsync(Guid transactionId);
    public Task<IResult<IList<Transaction>>> GetStatementByPaymentAccountIdAsync(Guid paymentAccountId);
    public Task<IResult<decimal>> GetBalanceByPaymentAccountIdAsync(Guid paymentAccountId);
    public IResult<Guid> UpdateTransaction(Transaction transaction);
}
