namespace Expensifier.Common.Domain;

public interface IApplyEvent<in TDomainEvent>
    where TDomainEvent : DomainEvent
{
    void Apply(TDomainEvent @event);
}