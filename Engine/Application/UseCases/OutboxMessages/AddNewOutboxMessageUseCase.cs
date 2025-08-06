using Engine.Application.Common.Results;
using Engine.Application.Interfaces;
using Engine.Application.Interfaces.Repositories;
using Engine.Application.Requests.OutboxMessages;
using Engine.Domain.Entities;

namespace Engine.Application.UseCases.OutboxMessages;

// DEPRECATED
public class AddNewOutboxMessageUseCase(
    IUnitOfWork unitOfWork,
    IOutboxMessageRepository repository)
{
    public async Task<IResult<Guid>> Execute(OutboxMessageRequest request)
    {
        try
        {
            OutboxMessage message = OutboxMessage.Create(
                request.Event,
                request.AggregateId,
                request.EventName,
                request.OccurredOn);

            await repository.AddAsync(message);

            await unitOfWork.SaveChangesAsync();

            return Result.Success(message.Id);
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>($"An error occurred while adding the outbox message: {ex.Message}");
        }
    }
}
