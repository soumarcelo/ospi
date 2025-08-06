using Engine.Application.Common.Results;
using Engine.Domain.ValueObjects;

namespace Engine.Application.Interfaces;

public interface IPaymentAccountBalanceService
{
    public Task<IResult<decimal>> GetBalanceAsync(Guid accountId);
    public Task<IResult<bool>> CanWithdrawAmountFromAccountAsync(Guid accountId, Money amount);
}
