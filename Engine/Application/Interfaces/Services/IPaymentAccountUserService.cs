using Engine.Application.Common.Results;
using Engine.Domain.Entities;

namespace Engine.Application.Interfaces.Services;

public interface IPaymentAccountUserService
{
    public Task<IResult<PaymentAccountUser>> AddPaymentAccountUserAsync(Guid paymentAccountId, Guid userId);
    public Task<IResult<PaymentAccountUser>> GetPaymentAccountUserByIdAsync(Guid paymentAccountUserId);
}
