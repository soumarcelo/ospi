using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
using Engine.Application.Interfaces;
using Engine.Application.Interfaces.Services;
using Engine.Application.Requests.OutboxMessages;
using Engine.Application.Requests.Transactions;
using Engine.Domain.Entities;
using Engine.Domain.Events.Transactions.Pix;

namespace Engine.Application.UseCases.EventHandlers.Transactions.Pix;

public class ProcessInternalPixTransferDebitRequestedEventUseCase(
    IUnitOfWork unitOfWork,
    IInternalTransferValidationService internalTransferValidationService,
    ITransactionService transactionService,
    IOutboxMessageService outboxMessageService)
{
    public async Task<IResult<Guid>> Execute(InternalPixTransferDebitRequestedEvent @event)
    {
        if (@event.FromAccountId == @event.ToAccountId)
        {
            return Result.Failure<Guid>(
                "The source and destination accounts cannot be the same for an internal transfer.");
        }

        IResult<Transaction> resultTransaction =
            await transactionService.GetTransactionByIdAsync(@event.TransactionId);
        if (!resultTransaction.TryGetValue(out Transaction outgoingTransaction, out Error? error))
        {
            return Result.Failure<Guid>(
                $"Error retrieving transaction with ID {@event.TransactionId}: {error?.Message}.");
        }

        if (!outgoingTransaction.IsPending())
        {
            return Result.Failure<Guid>(
                $"Transaction with ID {@event.TransactionId} is not in a pending state.");
        }

        IResult<User> resultInitiatorUser =
            await internalTransferValidationService.ValidateInitiatorUser(@event.InitiatorUserId);

        if (!resultInitiatorUser.TryGetValue(out User initiatorUser, out error))
        {
            return Result.Failure<Guid>(
                $"Error validating initiator user: {error?.Message}");
        }

        IResult<PaymentAccount> resultFromAccount =
            await internalTransferValidationService.ValidateDebitCounterparty(
                initiatorUser.Id,
                @event.FromAccountId,
                @event.Amount);

        if (!resultFromAccount.TryGetValue(out PaymentAccount fromAccount, out error))
        {
            return Result.Failure<Guid>(
                $"Error validating source account: {error?.Message}");
        }

        IResult<PaymentAccount> resultToAccount =
            await internalTransferValidationService.ValidateCreditCounterparty(
                @event.ToAccountId);

        if (!resultToAccount.TryGetValue(out PaymentAccount toAccount, out error))
        {
            return Result.Failure<Guid>(
                $"Error validating destination account: {error?.Message}");
        }

        await unitOfWork.BeginTransactionAsync();

        outgoingTransaction.Complete();

        IResult<Guid> resultOutgoingTransaction = 
            transactionService.UpdateTransaction(outgoingTransaction);
        
        if (!resultOutgoingTransaction.TryGetValue(out Guid _, out error))
        {
            await unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Guid>(
                $"Failed to update outgoing transaction: {error?.Message}");
        }

        PixIncomingTransactionRequest transactionRequest = new(
            initiatorUser,
            fromAccount,
            toAccount,
            @event.Amount,
            @event.E2EId,
            @event.Description);

        IResult<Transaction> resultPixIncoming =
            await transactionService.AddTransactionAsync(transactionRequest);

        if (!resultPixIncoming.TryGetValue(out Transaction incomingTransaction, out error))
        {
            await unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Guid>(
                $"Failed to create incoming transaction: {error?.Message}");
        }

        InternalPixTransferCreditRequestedEvent transactionEvent = new(
                outgoingTransaction.Id,
                incomingTransaction.Id,
                initiatorUser.Id,
                fromAccount.Id,
                toAccount.Id,
                @event.Amount,
                @event.E2EId,
                @event.Description);

        OutboxMessageRequest outboxMessageRequest = new(
            transactionEvent,
            incomingTransaction.Id,
            nameof(InternalPixTransferCreditRequestedEvent),
            transactionEvent.OccurredOn);

        IResult<OutboxMessage> resultOutboxMessage =
            await outboxMessageService.AddOutboxMessageAsync(outboxMessageRequest);

        if (!resultOutboxMessage.TryGetValue(out OutboxMessage outboxMessage, out error))
        {
            await unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Guid>(
                $"Error creating outbox message: {error?.Message}");
        }

        try
        {
            await unitOfWork.CommitTransactionAsync();
            return Result.Success(outboxMessage.Id);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Guid>(
                $"An error occurred while processing the internal pix debit transaction: {ex.Message}");
        }

    }
}
