using Engine.Application.Common.Results;
using Engine.Application.Interfaces.Repositories;
using Engine.Application.Interfaces.Services;
using Engine.Domain.Entities;
using Engine.Domain.ValueObjects;

namespace Engine.Application.Services;

public class PaymentMethodService(IPaymentMethodRepository repository) : IPaymentMethodService
{
    public async Task<IResult<PaymentMethod>> AddPaymentMethodAsync(
        Guid paymentAccountId,
        BankDetails details)
    {
        PaymentMethod paymentMethod = PaymentMethod.CreateBankTransfer(paymentAccountId, details);
        await repository.AddAsync(paymentMethod);
        return Result.Success(paymentMethod);
    }

    public async Task<IResult<PaymentMethod>> AddPaymentMethodAsync(
        Guid paymentAccountId,
        PixDetails details)
    {
        PaymentMethod paymentMethod = PaymentMethod.CreatePix(paymentAccountId, details);
        await repository.AddAsync(paymentMethod);
        return Result.Success(paymentMethod);
    }

    public async Task<IResult<PaymentMethod>> GetPaymentMethodByIdAsync(Guid paymentMethodId)
    {
        PaymentMethod? paymentMethod = await repository.GetByIdAsync(paymentMethodId);
        if (paymentMethod is null)
        {
            return Result.Failure<PaymentMethod>("Payment method not found");
        }
        return Result.Success(paymentMethod);
    }

    public async Task<IResult<IEnumerable<PaymentMethod>>> GetAllPaymentMethodsByAccountIdAsync(Guid accountId)
    {
        IEnumerable<PaymentMethod> paymentMethods = await repository.GetByAccountIdAsync(accountId);
        return Result.Success(paymentMethods);
    }

    public IResult<Guid> UpdatePaymentMethod(PaymentMethod paymentMethod)
    {
        repository.Update(paymentMethod);
        return Result.Success(paymentMethod.Id);
    }
}
