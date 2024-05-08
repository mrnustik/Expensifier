using Expensifier.Common.Domain;

namespace Expensifier.Common.Helpers;

internal static class EventApplicator
{
    public static void ApplyEvent(object @object, DomainEvent domainEvent)
    {
        var objectType = @object.GetType();
        var domainEventType = domainEvent.GetType();
        var applyEventType = typeof(IApplyEvent<>);
        var applyConcreteDomainEventType = applyEventType.MakeGenericType(domainEventType);
        if (objectType.IsAssignableTo(applyConcreteDomainEventType))
        {
            var applyMethodInfo = applyConcreteDomainEventType.GetMethod("Apply");
            applyMethodInfo!.Invoke(@object, [domainEvent]);
        }
        else
        {
            throw new InvalidOperationException(
                $"Can not apply domain event of type {domainEventType.Name} on type {objectType.Name}");
        }
    }
}