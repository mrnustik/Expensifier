using Expensifier.API.Common.Users;

namespace Expensifier.IntegrationTests;

public class TestUserProvider : IUserProvider
{
    public UserId CurrentUserId { get; } = UserId.New();
}