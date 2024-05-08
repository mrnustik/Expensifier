using Expensifier.API.Accounts.Domain;
using Expensifier.API.Common.Queries;

namespace Expensifier.API.Accounts.GetAccountsById;

public record GetAccountByIdQuery(AccountId AccountId) : Query<Account>;