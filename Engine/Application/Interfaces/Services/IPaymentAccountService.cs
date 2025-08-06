using Engine.Application.Common.Results;
using Engine.Domain.Entities;
using Engine.Domain.ValueObjects;

namespace Engine.Application.Interfaces.Services;

public interface IPaymentAccountService
{
    public Task<IResult<PaymentAccount>> AddPaymentAccountAsync(IndividualDocument document, string legalName);
    public Task<IResult<PaymentAccount>> AddPaymentAccountAsync(BusinessDocument document, string legalName);
    public Task<IResult<PaymentAccount>> GetPaymentAccountByIdAsync(Guid paymentAccountId);
    //public Task<IResult<IEnumerable<PaymentAccount>>> GetAllPaymentAccountsAsync();
    public IResult<Guid> UpdatePaymentAccount(PaymentAccount paymentAccount);
}
