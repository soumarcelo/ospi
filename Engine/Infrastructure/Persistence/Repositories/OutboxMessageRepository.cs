using Engine.Application.Interfaces.Repositories;
using Engine.Domain.Entities;
using Engine.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Engine.Infrastructure.Persistence.Repositories;

public class OutboxMessageRepository(AppDbContext _context) : IOutboxMessageRepository
{
    public async Task<OutboxMessage?> GetByIdAsync(Guid id)
    {
        return await _context.OutboxMessages.FindAsync(id);
    }

    public async Task<IEnumerable<OutboxMessage>> GetPendingMessagesAsync(int maxCount)
    {
        return await _context.OutboxMessages
            .Where(m => m.Status == OutboxMessageStatus.Pending)
            .OrderBy(m => m.CreatedAt)
            .Take(maxCount)
            .ToListAsync();
    }

    public async Task AddAsync(OutboxMessage outboxMessage)
    {
        await _context.OutboxMessages.AddAsync(outboxMessage);
    }

    public void Update(OutboxMessage outboxMessage)
    {
        _context.OutboxMessages.Update(outboxMessage);
    }

    public async Task DeleteAsync(Guid outboxMessageId)
    {
        OutboxMessage? outboxMessage = await GetByIdAsync(outboxMessageId) ??
            throw new KeyNotFoundException($"Outbox message with ID {outboxMessageId} not found.");
        _context.OutboxMessages.Remove(outboxMessage);
    }
}
