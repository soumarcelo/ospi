using Engine.Application.Common.Errors;
using Engine.Application.Common.Results;
using Engine.Application.Interfaces;
using Engine.Application.Interfaces.Repositories;
using Engine.Application.Interfaces.Services;
using Engine.Domain.Entities;
using Engine.Domain.Enums;
using Engine.Domain.Interfaces;

namespace Engine.Application.UseCases.OutboxMessages;

public class PublishOutboxMessageUseCase(
    IUnitOfWork unitOfWork,
    IOutboxMessageService outboxMessageService,
    IOutboxMessageRepository outboxMessageRepository,
    IEventDeserializerService eventDeserilizerService,
    IEventPublisher eventPublisher)
{
    public async Task<IResult<Guid>> Execute(Guid outboxMessageId)
    {
        IResult<OutboxMessage> result = await outboxMessageService.GetByIdAsync(outboxMessageId);
        if (!result.TryGetValue(out OutboxMessage outboxMessage, out Error? error))
        {
            return Result.Failure<Guid>($"Failed to retrieve outbox message: {error?.Message}");
        }

        if (outboxMessage.Status != OutboxMessageStatus.Pending)
        {
            return Result.Failure<Guid>("Outbox message is not in a pending state.");
        }

        outboxMessage.IncrementAttempts();

        if (outboxMessage.Attempts > 3)
        {
            outboxMessage.Failure(
                OutboxMessageFailureReasonCode.ExceededMaxAttempts,
                "Exceeded maximum attempts.");

            outboxMessageService.UpdateOutboxMessage(outboxMessage);
            await unitOfWork.SaveChangesAsync();

            return Result.Failure<Guid>("Failed to publish the transaction after multiple attempts.");
        }

        await unitOfWork.BeginTransactionAsync();

        try
        {
            outboxMessage.Publish();

            IDomainEvent @event =
                eventDeserilizerService.Deserialize<IDomainEvent>(
                    outboxMessage.MessageType,
                    outboxMessage.Data);

            await eventPublisher.PublishAsync(@event);

            outboxMessageService.UpdateOutboxMessage(outboxMessage);
            await unitOfWork.CommitTransactionAsync();

            return Result.Success(outboxMessage.Id);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync();
            return Result.Failure<Guid>($"An error occurred while publishing the transaction: {ex.Message}");
        }
    }
}
