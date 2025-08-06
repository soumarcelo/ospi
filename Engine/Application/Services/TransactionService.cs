using Engine.Application.Common.Results;
using Engine.Application.Interfaces.Repositories;
using Engine.Application.Interfaces.Services;
using Engine.Application.Requests.Transactions;
using Engine.Domain.Entities;
using Engine.Domain.Enums;

namespace Engine.Application.Services;

public class TransactionService(ITransactionRepository repository) : ITransactionService
{
    public async Task<IResult<Transaction>> AddTransactionAsync(PixOutgoingTransactionRequest request)
    {
        try
        {
            Transaction transaction = 
                Transaction.CreatePixOutgoingToInternal(
                    request.InitiatorUser.Id,
                    request.FromAccount.Id,
                    request.ToAccount.Id,
                    request.Amount,
                    TransactionType.Transfer,
                    request.E2EId,
                    request.Description);

            await repository.AddAsync(transaction);
            return Result.Success(transaction);
        }
        catch (Exception ex)
        {
            return Result.Failure<Transaction>(
                $"An error occurred while adding the transaction: {ex.Message}");
        }
    }

    public async Task<IResult<Transaction>> AddTransactionAsync(PixIncomingTransactionRequest request)
    {
        try
        {
            Transaction transaction =
                Transaction.CreatePixIncomingFromInternal(
                    request.InitiatorUser.Id,
                    request.FromAccount.Id,
                    request.ToAccount.Id,
                    request.Amount,
                    TransactionType.Transfer,
                    request.E2EId,
                    request.Description);
            await repository.AddAsync(transaction);
            return Result.Success(transaction);
        }
        catch (Exception ex)
        {
            return Result.Failure<Transaction>(
                $"An error occurred while adding the internal transfer credit transaction: {ex.Message}");
        }
    }

    public async Task<IResult<Transaction>> GetTransactionByIdAsync(Guid transactionId)
    {
        if (transactionId == Guid.Empty)
        {
            return Result.Failure<Transaction>("Invalid transaction ID provided.");
        }

        try
        {
            Transaction? transaction = await repository.GetByIdAsync(transactionId);
            if (transaction is null)
            {
                return Result.Failure<Transaction>("Transaction not found.");
            }
            return Result.Success(transaction);
        }
        catch (Exception ex)
        {
            return Result.Failure<Transaction>($"An error occurred while retrieving the transaction: {ex.Message}");
        }
    }

    public async Task<IResult<IList<Transaction>>> GetStatementByPaymentAccountIdAsync(Guid paymentAccountId)
    {
        if (paymentAccountId == Guid.Empty)
        {
            return Result.Failure<IList<Transaction>>("Invalid payment account ID provided.");
        }

        try
        {
            IList<Transaction> transactions = 
                await repository.GetStatementTransactionsByAccountIdAsync(paymentAccountId);
            return Result.Success(transactions);
        }
        catch (Exception ex)
        {
            return Result.Failure<IList<Transaction>>($"An error occurred while retrieving the statement: {ex.Message}");
        }
    }

    public async Task<IResult<decimal>> GetBalanceByPaymentAccountIdAsync(Guid paymentAccountId)
    {
        if (paymentAccountId == Guid.Empty)
        {
            return Result.Failure<decimal>("Invalid payment account ID provided.");
        }

        try
        {
            decimal balance = await repository.GetBalanceByAccountIdAsync(paymentAccountId);
            return Result.Success(balance);
        }
        catch (Exception ex)
        {
            return Result.Failure<decimal>($"An error occurred while retrieving the balance: {ex.Message}");
        }
    }

    public IResult<Guid> UpdateTransaction(Transaction transaction)
    {
        if (transaction is null)
        {
            return Result.Failure<Guid>("Transaction cannot be null.");
        }

        try
        {
            repository.Update(transaction);
            return Result.Success(transaction.Id);
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>($"An error occurred while updating the transaction: {ex.Message}");
        }
    }
}
