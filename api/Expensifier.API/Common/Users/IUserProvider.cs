namespace Expensifier.API.Common.Users;

public interface IUserProvider
{
    UserId CurrentUserId { get; }
}