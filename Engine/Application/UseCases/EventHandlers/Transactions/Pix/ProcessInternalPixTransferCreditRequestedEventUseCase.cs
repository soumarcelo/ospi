using Engine.Application.Common.Results;
using Engine.Application.Interfaces.Services;
using Engine.Application.Interfaces;
using Engine.Domain.Events.Transactions.Pix;
using Engine.Domain.Entities;
using Engine.Application.Common.Errors;

namespace Engine.Application.UseCases.EventHandlers.Transactions.Pix;

public class ProcessInternalPixTransferCreditRequestedEventUseCase(
    IUnitOfWork unitOfWork,
    IInternalTransferValidationService internalTransferValidationService,
    ITransactionService transactionService)
{
    public async Task<IResult<Guid>> Execute(InternalPixTransferCreditRequestedEvent @event)
    {
        if (@event.FromAccountId == @event.ToAccountId)
        {
            return Result.Failure<Guid>(
                "The source and destination accounts cannot be the same for an internal transfer.");
        }

        IResult<Transaction> resultOutgoingTransaction =
            await transactionService.GetTransactionByIdAsync(@event.OutgoingTransactionId);
        if (!resultOutgoingTransaction.TryGetValue(out Transaction outgoingTransaction, out Error? error))
        {
            return Result.Failure<Guid>(
                $"Error retrieving outgoing transaction with ID {@event.OutgoingTransactionId}: {error?.Message}.");
        }

        if (!outgoingTransaction.IsCompleted())
        {
            return Result.Failure<Guid>(
                $"Outgoing Transaction with ID {@event.OutgoingTransactionId} is not in a complete state.");
        }

        IResult<Transaction> resultTransaction =
            await transactionService.GetTransactionByIdAsync(@event.IncomingTransactionId);
        if (!resultTransaction.TryGetValue(out Transaction incomingTransaction, out error))
        {
            return Result.Failure<Guid>(
                $"Error retrieving incoming transaction with ID {@event.IncomingTransactionId}: {error?.Message}.");
        }

        if (!incomingTransaction.IsPending())
        {
            return Result.Failure<Guid>(
                $"Incoming Transaction with ID {@event.IncomingTransactionId} is not in a pending state.");
        }

        IResult<PaymentAccount> resultToAccount =
            await internalTransferValidationService.ValidateCreditCounterparty(
                @event.ToAccountId);
        if (!resultToAccount.TryGetValue(out PaymentAccount _, out error))
        {
            return Result.Failure<Guid>(
                $"Error validating destination account: {error?.Message}");
        }

        await unitOfWork.BeginTransactionAsync();

        incomingTransaction.Complete();

        IResult<Guid> resultUpdatedTransaction =
            transactionService.UpdateTransaction(incomingTransaction);
        if (!resultUpdatedTransaction.TryGetValue(out Guid incomingTransactionId, out error))
        {
            await unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Guid>(
                $"Error updating incoming transaction: {error?.Message ?? "Failed to update transaction"}");
        }

        try
        {
            await unitOfWork.CommitTransactionAsync();
            return Result.Success(incomingTransactionId);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Guid>($"Error processing credit transfer: {ex.Message}");
        }
    }
}