using Engine.Application.Common.Results;
using Engine.Application.Interfaces.Repositories;
using Engine.Application.Interfaces.Services;
using Engine.Application.Requests.OutboxMessages;
using Engine.Domain.Entities;

namespace Engine.Application.Services;

public class OutboxMessageService(IOutboxMessageRepository repository) : IOutboxMessageService
{
    public async Task<IResult<OutboxMessage>> AddOutboxMessageAsync(OutboxMessageRequest request)
    {
        try
        {
            OutboxMessage outboxMessage = OutboxMessage.Create(
                request.Event,
                request.AggregateId,
                request.EventName,
                request.OccurredOn);

            await repository.AddAsync(outboxMessage);
            return Result.Success(outboxMessage);
        }
        catch
        {
            return Result.Failure<OutboxMessage>(
                $"An error occurred while adding the outbox message for event {request.EventName}.");
        }
    }

    public async Task<IResult<OutboxMessage>> GetByIdAsync(Guid outboxMessageId)
    {
        try
        {
            OutboxMessage? outboxMessage = await repository.GetByIdAsync(outboxMessageId);
            if (outboxMessage == null)
            {
                return Result.Failure<OutboxMessage>($"Outbox message with ID {outboxMessageId} not found.");
            }
            return Result.Success(outboxMessage);
        }
        catch
        {
            return Result.Failure<OutboxMessage>(
                $"An error occurred while retrieving the outbox message with ID {outboxMessageId}.");
        }
    }

    public IResult<Guid> UpdateOutboxMessage(OutboxMessage outboxMessage)
    {
        try
        {
            repository.Update(outboxMessage);
            return Result.Success(outboxMessage.Id);
        }
        catch
        {
            return Result.Failure<Guid>(
                $"An error occurred while updating the outbox message with ID {outboxMessage.Id}.");
        }
    }
}
