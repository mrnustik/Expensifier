using Expensifier.API.Accounts.Domain;
using Expensifier.API.Common.Users;
using Marten.Events.Aggregation;

namespace Expensifier.API.Accounts.GetAccounts;

public record AccountListItem
{
    public Guid Id { get; set; }
    public AccountId AccountId => new(Id);
    public UserId UserId { get; set; }
    public string Name { get; set; }

    private void Apply(AccountCreated @event)
    {
        Id = @event.Id.Value;
        Name = @event.Name;
        UserId = @event.UserId;
    }

    public class ProjectionConfiguration : SingleStreamProjection<AccountListItem>
    {
        public ProjectionConfiguration()
        {
            ProjectEvent<AccountCreated>((a, e) => a.Apply(e));
        }
    }
}