using Expensifier.API.Accounts.Domain;
using Marten.Events.Aggregation;

namespace Expensifier.API.Accounts.GetAccountById;

public class AccountDetail
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }

    private void Apply(AccountCreated @event)
    {
        Id = @event.Id.Value;
        Name = @event.Name;
        UserId = @event.UserId.Value;
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