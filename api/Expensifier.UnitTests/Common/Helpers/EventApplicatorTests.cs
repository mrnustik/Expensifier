using Expensifier.Common.Domain;
using Expensifier.Common.Helpers;
using FluentAssertions;

namespace Expensifier.UnitTests.Common.Helpers;

public class EventApplicatorTests
{
    [Fact]
    public void ApplyEvent_WithExistingApplyEventImplementation_ShouldApplyIt()
    {
        // Arrange
        var eventData = "Test";
        var testDomainEvent = new TestDomainEvent(eventData);
        var objectThatApplies = new ObjectThatApplies();

        // Act
        EventApplicator.ApplyEvent(objectThatApplies, testDomainEvent);

        // Assert
        objectThatApplies.Information
                         .Should()
                         .Be(eventData);
    }

    [Fact]
    public void ApplyEvent_WithMissingApplyEventImplementation_ShouldThrow()
    {
        // Arrange
        var eventData = "Test";
        var testDomainEvent = new TestDomainEvent(eventData);
        var objectThatDoesNotApply = new ObjectThatDoesNotApply();

        // Act
        var action = () => EventApplicator.ApplyEvent(objectThatDoesNotApply, testDomainEvent);

        // Assert
        action.Should()
              .Throw<InvalidOperationException>()
              .Which
              .Message
              .Should()
              .Be($"Can not apply domain event of type {nameof(TestDomainEvent)} on type {nameof(ObjectThatDoesNotApply)}");
    }

    public record TestDomainEvent(string Information) : DomainEvent;

    public record OtherDomainEvent(int Value) : DomainEvent;

    public class ObjectThatApplies : IApplyEvent<TestDomainEvent>,
                                     IApplyEvent<OtherDomainEvent>
    {
        public string Information { get; private set; } = string.Empty;

        public void Apply(TestDomainEvent @event)
        {
            Information = @event.Information;
        }

        public void Apply(OtherDomainEvent @event)
        {
            Information = @event.Value.ToString();
        }
    }

    public class ObjectThatDoesNotApply : IApplyEvent<OtherDomainEvent>
    {
        public void Apply(OtherDomainEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}