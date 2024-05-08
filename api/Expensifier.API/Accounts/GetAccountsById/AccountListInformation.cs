using Expensifier.API.Accounts.Domain;
using Marten.Events.Aggregation;

namespace Expensifier.API.Accounts.GetAccountsById;

public class AccountListInformation
{
    public Guid Id { get; set; }
    public AccountId AccountId { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }

    public void Apply(AccountCreated accountCreated)
    {
        Id = accountCreated.Id.Value;
        AccountId = accountCreated.Id;
        Name = accountCreated.Name;
        Balance = 0;
    }

    public class ProjectionConfiguration : SingleStreamProjection<AccountListInformation>
    {
        public ProjectionConfiguration()
        {
            ProjectEvent<AccountCreated>((i, e) => i.Apply(e));
        }
    }
}