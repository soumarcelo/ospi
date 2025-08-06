namespace Engine.Domain.Interfaces;

public interface IDomainEvent
{
    public DateTime OccurredOn { get; }
}
