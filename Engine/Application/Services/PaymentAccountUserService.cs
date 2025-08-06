using Engine.Application.Common.Results;
using Engine.Application.Interfaces.Repositories;
using Engine.Application.Interfaces.Services;
using Engine.Domain.Entities;

namespace Engine.Application.Services;

public class PaymentAccountUserService(IPaymentAccountUserRepository repository) : IPaymentAccountUserService
{
    public async Task<IResult<PaymentAccountUser>> AddPaymentAccountUserAsync(
        Guid paymentAccountId,
        Guid userId)
    {
        PaymentAccountUser paymentAccountUser = PaymentAccountUser.Create(paymentAccountId, userId);
        await repository.AddAsync(paymentAccountUser);
        return Result.Success(paymentAccountUser);
    }

    public async Task<IResult<PaymentAccountUser>> GetPaymentAccountUserByIdAsync(Guid paymentAccountUserId)
    {
        PaymentAccountUser? paymentAccountUser = await repository.GetByIdAsync(paymentAccountUserId);
        if (paymentAccountUser is null)
        {
            return Result.Failure<PaymentAccountUser>("Payment account user not found");
        }
        return Result.Success(paymentAccountUser);
    }
}
