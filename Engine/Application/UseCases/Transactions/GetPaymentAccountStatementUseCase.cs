using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
using Engine.Application.DTOs.Transactions;
using Engine.Application.Interfaces.Services;
using Engine.Domain.Entities;

namespace Engine.Application.UseCases.Transactions;

public class GetPaymentAccountStatementUseCase(
    IPaymentAccountService accountService,
    ITransactionService transactionService)
{
    public async Task<IResult<IList<StatementTransactionDTO>>> Execute(Guid paymentAccountId)
    {
        if (paymentAccountId == Guid.Empty)
        {
            return Result.Failure<IList<StatementTransactionDTO>>("Invalid payment account ID provided.");
        }

        IResult<PaymentAccount> accountResult = 
            await accountService.GetPaymentAccountByIdAsync(paymentAccountId);
        if (!accountResult.TryGetValue(out PaymentAccount? paymentAccount, out Error? error))
        {
            return Result.Failure<IList<StatementTransactionDTO>>(
                $"Failed to retrieve payment account: {error?.Message}");
        }

        IResult<IList<Transaction>> transactionsResult = 
            await transactionService.GetStatementByPaymentAccountIdAsync(paymentAccountId);
        if (!transactionsResult.TryGetValue(out IList<Transaction>? transactions, out error))
        {
            return Result.Failure<IList<StatementTransactionDTO>>(
                $"Failed to retrieve transactions: {error?.Message}");
        }
        IList<StatementTransactionDTO> statementTransactions = 
            [..transactions.Select(StatementTransactionDTO.FromEntity)];
        return Result.Success(statementTransactions);
    }
}
