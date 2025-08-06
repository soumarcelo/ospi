using Engine.Application.Common.Results;
using Engine.Domain.Entities;
using Engine.Domain.ValueObjects;

namespace Engine.Application.Interfaces.Services;

public interface IPaymentMethodService
{
    public Task<IResult<PaymentMethod>> AddPaymentMethodAsync(
        Guid paymentAccountId,
        BankDetails bankDetails);
    public Task<IResult<PaymentMethod>> AddPaymentMethodAsync(
        Guid paymentAccountId,
        PixDetails bankDetails);
    public Task<IResult<PaymentMethod>> GetPaymentMethodByIdAsync(Guid paymentMethodId);
    public Task<IResult<IEnumerable<PaymentMethod>>> GetAllPaymentMethodsByAccountIdAsync(Guid paymentAccountId);
    public IResult<Guid> UpdatePaymentMethod(PaymentMethod paymentMethod);
}
