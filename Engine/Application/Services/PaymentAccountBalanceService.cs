using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
using Engine.Application.Interfaces;
using Engine.Application.Interfaces.Services;
using Engine.Domain.ValueObjects;

namespace Engine.Application.Services;

public class PaymentAccountBalanceService(
    ITransactionService transactionService) : IPaymentAccountBalanceService
{
    public async Task<IResult<decimal>> GetBalanceAsync(Guid accountId)
    {
        if (accountId == Guid.Empty)
        {
            return Result.Failure<decimal>("Invalid account ID provided.");
        }

        IResult<decimal> balanceResult = await transactionService.GetBalanceByPaymentAccountIdAsync(accountId);
        if (!balanceResult.TryGetValue(out decimal balance, out Error? error))
        {
            return Result.Failure<decimal>($"Failed to retrieve balance: {error?.Message}");
        }

        return Result.Success(balance);
    }

    public async Task<IResult<bool>> CanWithdrawAmountFromAccountAsync(Guid accountId, Money amount)
    {
        if (accountId == Guid.Empty)
        {
            return Result.Failure<bool>("Invalid account ID provided.");
        }

        if (amount <= Money.Zero)
        {
            return Result.Failure<bool>("Withdrawal amount must be greater than zero.");
        }

        IResult<decimal> balanceResult = await transactionService.GetBalanceByPaymentAccountIdAsync(accountId);
        if (!balanceResult.TryGetValue(out decimal balance, out Error? error))
        {
            return Result.Failure<bool>($"Failed to retrieve balance: {error?.Message}");
        }

        Money _balance = new(balance);

        return Result.Success(_balance >= amount);
    }
}
