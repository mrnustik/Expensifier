using Expensifier.API.Accounts.Domain;
using Expensifier.API.Common.Users;
using Marten.Events.Aggregation;

namespace Expensifier.API.Accounts.GetAccountById;

public class AccountDetail
{
    public Guid Id { get; set; }
    public AccountId AccountId { get; set; }
    public string Name { get; set; }
    public UserId UserId { get; set; }
    public decimal Balance { get; set; }

    private void Apply(AccountCreated @event)
    {
        Id = @event.Id.Value;
        AccountId = @event.Id;
        Name = @event.Name;
        UserId = @event.UserId;
        Balance = 0;
    }

    public class ProjectionConfiguration : SingleStreamProjection<AccountDetail>
    {
        public ProjectionConfiguration()
        {
            ProjectEvent<AccountCreated>((i, e) => i.Apply(e));
        }
    }
}