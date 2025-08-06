using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
using Engine.Application.Interfaces;

namespace Engine.Application.UseCases.Transactions;

public class GetPaymentAccountBalanceUseCase(IPaymentAccountBalanceService balanceService)
{
    public async Task<IResult<decimal>> Execute(Guid accountId)
    {
        if (accountId == Guid.Empty)
        {
            return Result.Failure<decimal>("Invalid account ID provided.");
        }

        IResult<decimal> balanceResult = await balanceService.GetBalanceAsync(accountId);
        if (!balanceResult.TryGetValue(out decimal balance, out Error? error))
        {
            return Result.Failure<decimal>($"Failed to retrieve balance: {error?.Message}");
        }

        return Result.Success(balance);
    }
}
