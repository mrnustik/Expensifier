using Expensifier.Common.Helpers;

namespace Expensifier.Common.Domain;

public abstract record EventSourcedAggregate<TId> : Aggregate<TId>
    where TId : Id
{
    private Queue<DomainEvent> _events = new();

    protected EventSourcedAggregate(TId id) : base(id)
    {
    }

    public IEnumerable<DomainEvent> DequeueAllEvents()
    {
        while (_events.TryDequeue(out var @event))
        {
            yield return @event;
        }
    }

    protected void EnqueueAndApply<TEvent>(TEvent @event) where TEvent : DomainEvent
    {
        _events.Enqueue(@event);
        EventApplicator.ApplyEvent(this, @event);
    }
}