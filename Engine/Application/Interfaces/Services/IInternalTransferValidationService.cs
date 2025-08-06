using Engine.Application.Common.Results;
using Engine.Domain.Entities;
using Engine.Domain.ValueObjects;

namespace Engine.Application.Interfaces.Services;

public interface IInternalTransferValidationService
{
    public Task<IResult<User>> ValidateInitiatorUser(Guid initiatorUserId);
    public Task<IResult<PaymentAccount>> ValidateDebitCounterparty(
        Guid initiatorUserId,
        Guid paymentAccountId,
        Money amount);
    public Task<IResult<PaymentAccount>> ValidateCreditCounterparty(Guid paymentAccountId);
}
