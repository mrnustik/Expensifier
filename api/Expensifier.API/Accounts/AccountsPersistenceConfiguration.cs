using Expensifier.API.Accounts.GetAccountsById;
using Marten;
using Marten.Events.Projections;

namespace Expensifier.API.Accounts;

public static class AccountsPersistenceConfiguration
{
    public static void ConfigureAccounts(this StoreOptions storeOptions)
    {
        storeOptions.Projections.Add<AccountListInformation.ProjectionConfiguration>(ProjectionLifecycle.Inline);
    }
}