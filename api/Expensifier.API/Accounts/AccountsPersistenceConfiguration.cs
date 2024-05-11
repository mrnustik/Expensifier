using Expensifier.API.Accounts.GetAccountById;
using Expensifier.API.Accounts.GetAccounts;
using Marten;
using Marten.Events.Projections;

namespace Expensifier.API.Accounts;

public static class AccountsPersistenceConfiguration
{
    public static void ConfigureAccounts(this StoreOptions storeOptions)
    {
        storeOptions.Projections.Add<AccountDetail.ProjectionConfiguration>(ProjectionLifecycle.Inline);
        storeOptions.Projections.Add<AccountListItem.ProjectionConfiguration>(ProjectionLifecycle.Inline);
    }
}