namespace Financer.Domain.Entities;

public interface IDomainEvent
{
    Guid AggregateId { get; }
    DateTime OccurredOn { get; }
}
