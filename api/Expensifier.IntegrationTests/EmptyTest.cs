using FluentAssertions;

namespace Expensifier.IntegrationTests;

public class EmptyTest
{
    [Fact]
    public void True_Should_BeTrue()
    {
        true.Should()
            .BeTrue();
    }
}