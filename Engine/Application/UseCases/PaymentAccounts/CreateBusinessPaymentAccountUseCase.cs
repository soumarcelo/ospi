using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
using Engine.Application.DTOs.PaymentAccounts;
using Engine.Application.Interfaces;
using Engine.Application.Interfaces.Services;
using Engine.Domain.Entities;
using Engine.Domain.Enums;
using Engine.Domain.ValueObjects;

namespace Engine.Application.UseCases.PaymentAccounts;

public class CreateBusinessPaymentAccountUseCase(
    IUnitOfWork unitOfWork,
    IPaymentAccountService paymentAccountService,
    IPaymentAccountUserService paymentAccountUserService,
    IPaymentMethodService paymentMethodService,
    IAccountGeneratorService accountGeneratorService)
{
    public async Task<IResult<Guid>> Execute(Guid userId, CreateBusinessPaymentAccountDTO dto)
    {
        if (dto.Document == BusinessDocument.Invalid)
        {
            return Result.Failure<Guid>("Invalid document provided.");
        }

        if (string.IsNullOrWhiteSpace(dto.LegalName))
        {
            return Result.Failure<Guid>("Legal name cannot be empty.");
        }

        await unitOfWork.BeginTransactionAsync();

        //PaymentAccount paymentAccount = PaymentAccount.CreateBusiness();

        IResult<PaymentAccount> createAccountResult = 
            await paymentAccountService.AddPaymentAccountAsync(dto.Document, dto.LegalName);

        if (!createAccountResult.TryGetValue(out PaymentAccount createdAccount, out Error? error))
        {
            await unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Guid>($"Failed to create payment account: {error?.Message}");
        }

        //PaymentAccountUser paymentAccountUser = PaymentAccountUser.Create();

        IResult<PaymentAccountUser> createUserLinkResult = 
            await paymentAccountUserService.AddPaymentAccountUserAsync(createdAccount.Id, userId);

        if (!createUserLinkResult.TryGetValue(out PaymentAccountUser _, out error))
        {
            await unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Guid>($"Failed to link user to account: {error?.Message}");
        }

        string accountNumber = accountGeneratorService.GenerateAccountNumber();
        string checkDigit = accountGeneratorService.CalculateCheckDigit(accountNumber);

        BankDetails bankDetails = new(accountNumber, checkDigit);
        //PaymentMethod paymentMethod = PaymentMethod.CreateBankTransfer();

        IResult<PaymentMethod> createPaymentMethodResult =
            await paymentMethodService.AddPaymentMethodAsync(
                createdAccount.Id,
                bankDetails);

        if (!createPaymentMethodResult.TryGetValue(out PaymentMethod _, out error))
        {
            await unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Guid>($"Failed to create payment method: {error?.Message}");
        }

        try
        {
            await unitOfWork.CommitTransactionAsync();

            return Result.Success(createdAccount.Id);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Guid>($"An error occurred while creating the payment account: {ex.Message}");
        }
    }
}