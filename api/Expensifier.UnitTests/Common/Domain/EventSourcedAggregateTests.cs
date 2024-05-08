using Expensifier.Common.Domain;
using FluentAssertions;

namespace Expensifier.UnitTests.Common.Domain;

public class EventSourcedAggregateTests
{
    [Fact]
    public void EnqueueAndApply_WithValidEvent_AppliesIt()
    {
        // Arrange
        var id = new StringId("id");
        var aggregate = new TestAggregate(id);

        // Act
        aggregate.DoEventAction(42);

        // Assert
        aggregate.Information
                 .Should()
                 .Be(42);
    }

    [Fact]
    public void DequeueAllEvents_WithMultipleEvents_PreserversOrder()
    {
        // Arrange
        var id = new StringId("id");
        var aggregate = new TestAggregate(id);

        // Act
        aggregate.DoEventAction(0);
        aggregate.DoEventAction(1);
        aggregate.DoEventAction(2);
        var dequeuedEvents = aggregate.DequeueAllEvents();

        // Assert
        dequeuedEvents
            .Cast<TestDomainEvent>()
            .Select(e => e.Information)
            .Should()
            .BeEquivalentTo([0, 1, 2]);
    }

    [Fact]
    public void DequeueAllEvents_WithNoEvents_ReturnsNone()
    {
        // Arrange
        var id = new StringId("id");
        var aggregate = new TestAggregate(id);

        // Act
        var dequeuedEvents = aggregate.DequeueAllEvents();

        // Assert
        dequeuedEvents
            .Should()
            .BeEmpty();
    }

    private record StringId(string StringRepresentation)
        : Id;

    private record TestDomainEvent(int Information) : DomainEvent;

    private record TestAggregate : EventSourcedAggregate<StringId>,
                                   IApplyEvent<TestDomainEvent>
    {
        public TestAggregate(StringId id) : base(id)
        {
        }

        public int Information { get; private set; }

        public void DoEventAction(int information)
        {
            EnqueueAndApply(new TestDomainEvent(information));
        }

        public void Apply(TestDomainEvent @event)
        {
            Information = @event.Information;
        }
    }
}