namespace Engine.Application.Interfaces;

public interface IEventHandler<TKey, TValue>
{
    public Task ProcessMessageAsync(
        TKey key, TValue value, CancellationToken cancellationToken = default);
}
