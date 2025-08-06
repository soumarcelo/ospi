using Engine.Application.Common.Results;
using Engine.Application.Requests.OutboxMessages;
using Engine.Domain.Entities;

namespace Engine.Application.Interfaces.Services;

public interface IOutboxMessageService
{
    public Task<IResult<OutboxMessage>> AddOutboxMessageAsync(OutboxMessageRequest request);
    public Task<IResult<OutboxMessage>> GetByIdAsync(Guid outboxMessageId);
    public IResult<Guid> UpdateOutboxMessage(OutboxMessage outboxMessage);
}
