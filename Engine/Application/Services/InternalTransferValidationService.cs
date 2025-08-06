using Engine.Application.Common.Results;
using Engine.Application.Interfaces;
using Engine.Application.Interfaces.Repositories;
using Engine.Application.Interfaces.Services;
using Engine.Domain.Entities;
using Engine.Domain.ValueObjects;

namespace Engine.Application.Services;

public class InternalTransferValidationService(
    IUserRepository userRepository,
    IPaymentAccountRepository paymentAccountRepository,
    IPaymentAccountUserRepository paymentAccountUserRepository,
    IPaymentAccountBalanceService paymentAccountBalanceService) : IInternalTransferValidationService
{
    private async Task<IResult<PaymentAccount>> GetPaymentAccount(Guid accountId)
    {
        PaymentAccount? account = await paymentAccountRepository.GetByIdAsync(accountId);
        if (account is null)
        {
            return Result.Failure<PaymentAccount>(
                $"Payment account with ID {accountId} not found.");
        }

        return Result.Success(account);
    }

    public async Task<IResult<User>> ValidateInitiatorUser(Guid initiatorUserId)
    {
        User? user = await userRepository.GetByIdAsync(initiatorUserId);
        if (user is null)
        {
            return Result.Failure<User>($"User with ID {initiatorUserId} not found.");
        }
        return Result.Success(user);
    }

    public async Task<IResult<PaymentAccount>> ValidateDebitCounterparty(
        Guid initiatorUserId,
        Guid paymentAccountId,
        Money amount)
    {
        if (!amount.IsValid)
        {
            return Result.Failure<PaymentAccount>(
                "The transaction amount must be greater than zero.");
        }

        IResult<PaymentAccount> accountResult = await GetPaymentAccount(paymentAccountId);
        if (!accountResult.IsSuccess || accountResult.Value is null)
        {
            return Result.Failure<PaymentAccount>($"{accountResult.ErrorMessage}");
        }
        
        PaymentAccount account = accountResult.Value;
        if (!account.CanTransact())
        {
            return Result.Failure<PaymentAccount>(
                "The account is not allowed to transact at this time.");
        }

        bool canUserWithdraw = 
            await paymentAccountUserRepository.CanUserTransactWithAccount(
                initiatorUserId,
                paymentAccountId);
        if (!canUserWithdraw)
        {
            return Result.Failure<PaymentAccount>(
                "The user is not allowed to withdraw from this account.");
        }

        IResult<bool> resultCanWithdrawAmount = 
            await paymentAccountBalanceService.CanWithdrawAmountFromAccountAsync(
                account.Id,
                amount);

        if (!resultCanWithdrawAmount.IsSuccess)
        {
            return Result.Failure<PaymentAccount>(
                $"Error checking account balance: {resultCanWithdrawAmount.ErrorMessage}");
        }

        bool canWithdrawAmount = resultCanWithdrawAmount.Value;
        if (!canWithdrawAmount)
        {
            return Result.Failure<PaymentAccount>(
                "Insufficient funds in the account to process the transaction.");
        }

        return Result.Success(account);
    }

    public async Task<IResult<PaymentAccount>> ValidateCreditCounterparty(Guid paymentAccountId)
    {
        IResult<PaymentAccount> accountResult = await GetPaymentAccount(paymentAccountId);
        if (!accountResult.IsSuccess || accountResult.Value is null)
        {
            return Result.Failure<PaymentAccount>($"{accountResult.ErrorMessage}");
        }

        PaymentAccount account = accountResult.Value;
        if (!account.CanTransact())
        {
            return Result.Failure<PaymentAccount>(
                "The destination account is not allowed to transact at this time.");
        }

        return Result.Success(account);
    }
}
