using Engine.Domain.Interfaces;

namespace Engine.Application.Interfaces;

public interface IEventPublisher
{
    public Task PublishAsync<TEvent>(TEvent data) where TEvent : IDomainEvent;
}
