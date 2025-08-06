using Engine.Domain.Entities;

namespace Engine.Application.Interfaces.Repositories;

public interface IOutboxMessageRepository : IRepository<OutboxMessage>
{
    public Task<IEnumerable<OutboxMessage>> GetPendingMessagesAsync(int maxCount);
}
