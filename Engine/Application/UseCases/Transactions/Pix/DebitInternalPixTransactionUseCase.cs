using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
using Engine.Application.DTOs.Transactions.Pix;
using Engine.Application.Interfaces;
using Engine.Application.Interfaces.Services;
using Engine.Application.Requests.OutboxMessages;
using Engine.Application.Requests.Transactions;
using Engine.Domain.Entities;
using Engine.Domain.Events.Transactions.Pix;
using Engine.Domain.ValueObjects;

namespace Engine.Application.UseCases.Transactions.Pix;

public class DebitInternalPixTransactionUseCase(
    IUnitOfWork unitOfWork,
    IInternalTransferValidationService internalTransferValidationService,
    ITransactionService transactionService,
    IOutboxMessageService outboxMessageService)
{
    public async Task<IResult<Guid>> Execute(CreatePixTransactionDTO dto)
    {
        if (dto.FromAccountId == dto.ToAccountId)
        {
            return Result.Failure<Guid>(
                "The source and destination accounts cannot be the same for a debit transaction.");
        }

        IResult<User> resultInitiatorUser =
            await internalTransferValidationService.ValidateInitiatorUser(dto.InitiatorUserId);

        if (!resultInitiatorUser.TryGetValue(out User initiatorUser, out Error? error))
        {
            return Result.Failure<Guid>(
                $"Error validating initiator user: {error?.Message ?? $"Initiator user with ID {dto.InitiatorUserId} not found."}");
        }

        IResult<PaymentAccount> resultFromAccount =
            await internalTransferValidationService.ValidateDebitCounterparty(
                initiatorUser.Id,
                dto.FromAccountId,
                dto.Amount);

        if (!resultFromAccount.TryGetValue(out PaymentAccount fromAccount, out error))
        {
            return Result.Failure<Guid>(
                $"Error validating source account: {error?.Message ?? $"Source account with ID {dto.FromAccountId} not found."}");
        }

        IResult<PaymentAccount> resultToAccount =
            await internalTransferValidationService.ValidateCreditCounterparty(
                dto.ToAccountId);

        if (!resultToAccount.TryGetValue(out PaymentAccount toAccount, out error))
        {
            return Result.Failure<Guid>(
                $"Error validating destination account: {error?.Message ?? $"Destination account with ID {dto.ToAccountId} not found."}");
        }

        await unitOfWork.BeginTransactionAsync();

        EndToEndId e2eId = new();

        PixOutgoingTransactionRequest transactionRequest = new(
            initiatorUser,
            fromAccount,
            toAccount,
            dto.Amount,
            e2eId,
            dto.Description);

        IResult<Transaction> resultPixOutgoing =
            await transactionService.AddTransactionAsync(transactionRequest);

        if (!resultPixOutgoing.TryGetValue(out Transaction pixOutgoing, out error))
        {
            await unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Guid>(
                $"Error creating outgoing transaction: {error?.Message ?? "Failed to create transaction"}");
        }

        InternalPixTransferDebitRequestedEvent transactionEvent = new(
            pixOutgoing.Id,
            initiatorUser.Id,
            fromAccount.Id,
            toAccount.Id,
            dto.Amount,
            e2eId,
            dto.Description);

        OutboxMessageRequest outboxMessageRequest = new(
            transactionEvent,
            pixOutgoing.Id,
            nameof(InternalPixTransferDebitRequestedEvent),
            transactionEvent.OccurredOn);

        IResult<OutboxMessage> resultOutboxMessage =
            await outboxMessageService.AddOutboxMessageAsync(outboxMessageRequest);

        if (!resultOutboxMessage.TryGetValue(out OutboxMessage outboxMessage, out error))
        {
            await unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Guid>(
                $"Error creating outbox message: {error?.Message ?? "Failed to create outbox message"}");
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
                $"An error occurred while creating the internal pix debit transaction: {ex.Message}");
        }
    }
}