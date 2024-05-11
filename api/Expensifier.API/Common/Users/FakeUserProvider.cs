namespace Expensifier.API.Common.Users;

public class FakeUserProvider : IUserProvider
{
    public UserId CurrentUserId => UserId.Parse("96FC7C4B-70FD-4236-98FA-1BA3203B8CD4");
}