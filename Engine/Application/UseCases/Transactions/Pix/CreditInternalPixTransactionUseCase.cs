using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
using Engine.Application.Interfaces;
using Engine.Application.Interfaces.Services;
using Engine.Application.Requests.Transactions;
using Engine.Domain.Entities;
using Engine.Domain.Events.Transactions.Pix;

namespace Engine.Application.UseCases.Transactions.Pix;

// Injeta o serviço de transações em vez do repositório
public class CreditInternalPixTransactionUseCase(
    IUnitOfWork unitOfWork,
    ITransactionService transactionService)
{
    public async Task<IResult<Guid>> Execute(
        InternalTransferCreditTransactionRequest request)
    {
        //if (!request.Amount.IsValid)
        //{
        //    return Result.Failure<InternalPixTransferCreditRequestedEvent>(
        //        "The transaction amount must be greater than zero.");
        //}

        //await unitOfWork.BeginTransactionAsync();

        //IResult<Transaction> createTransactionResult =
        //    await transactionService.CreatePixIncomingFromInternalAsync(request);

        //if (!createTransactionResult.TryGetValue(out Transaction createdTransaction, out Error? error))
        //{
        //    await unitOfWork.RollbackTransactionAsync();
        //    return Result.Failure<InternalPixTransferCreditRequestedEvent>(
        //        $"Failed to create incoming pix transaction: {error?.Message}");
        //}

        //InternalPixTransferCreditRequestedEvent txEvent =
        //    new(
        //        request.OutgoingTransaction.Id,
        //        createdTransaction.Id,
        //        request.InitiatorUser.Id,
        //        request.FromAccount.Id,
        //        request.ToAccount.Id,
        //        request.Amount,
        //        request.E2EId,
        //        request.Description);

        //try
        //{
        //    // Tenta commitar a transação
        //    await unitOfWork.CommitTransactionAsync();
        //    return Result.Success(txEvent);
        //}
        //catch (Exception ex)
        //{
        //    // Se o commit falhar, faz rollback e retorna o erro
        //    await unitOfWork.RollbackTransactionAsync();
        //    return Result.Failure<InternalPixTransferCreditRequestedEvent>(
        //        $"An error occurred while processing the internal pix credit transaction: {ex.Message}");
        //}

        return Result.Success(Guid.NewGuid());
    }
}
