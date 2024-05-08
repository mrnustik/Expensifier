namespace Expensifier.Common.Domain;

public interface IEventSourcedRepository<TAggregate, in TId>
    where TAggregate : Aggregate<TId>
{
    Task<TAggregate> Load(TId id, CancellationToken cancellationToken);
    Task Store(TAggregate item, CancellationToken cancellationToken);
}