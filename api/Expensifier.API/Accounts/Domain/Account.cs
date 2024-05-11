using Expensifier.API.Common.Users;

namespace Expensifier.API.Accounts.Domain;

public class Account
{
    public Account()
    {
    }

    public Account(AccountCreated @event)
    {
        Id = @event.Id.Value;
        Name = @event.Name;
        UserId = @event.UserId;
        Balance = 0;
    }

    public Guid Id { get; set; }
    public string Name { get; }
    public decimal Balance { get; }
    public UserId UserId { get; }
}