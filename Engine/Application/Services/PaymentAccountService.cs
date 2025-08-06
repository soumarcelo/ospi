using Engine.Application.Common.Results;
using Engine.Application.Interfaces.Repositories;
using Engine.Application.Interfaces.Services;
using Engine.Domain.Entities;
using Engine.Domain.ValueObjects;

namespace Engine.Application.Services;

public class PaymentAccountService(IPaymentAccountRepository repository) : IPaymentAccountService
{
    public async Task<IResult<PaymentAccount>> AddPaymentAccountAsync(IndividualDocument document, string legalName)
    {
        PaymentAccount paymentAccount = PaymentAccount.CreateIndividual(document, legalName);
        await repository.AddAsync(paymentAccount);
        return Result.Success(paymentAccount);
    }

    public async Task<IResult<PaymentAccount>> AddPaymentAccountAsync(BusinessDocument document, string legalName)
    {
        PaymentAccount paymentAccount = PaymentAccount.CreateBusiness(document, legalName);
        await repository.AddAsync(paymentAccount);
        return Result.Success(paymentAccount);
    }

    public async Task<IResult<PaymentAccount>> GetPaymentAccountByIdAsync(Guid paymentAccountId)
    {
        PaymentAccount? paymentAccount = await repository.GetByIdAsync(paymentAccountId);
        if (paymentAccount is null)
        {
            return Result.Failure<PaymentAccount>("Payment account not found");
        }
        return Result.Success(paymentAccount);
    }

    //public async Task<IResult<IEnumerable<PaymentAccount>>> GetAllPaymentAccountsAsync()
    //{
    //    IEnumerable<PaymentAccount> paymentAccounts = await repository.GetAllAsync();
    //    return Result.Success(paymentAccounts);
    //}

    public IResult<Guid> UpdatePaymentAccount(PaymentAccount paymentAccount)
    {
        repository.Update(paymentAccount);
        return Result.Success(paymentAccount.Id);
    }
}
