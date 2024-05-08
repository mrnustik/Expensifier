using Expensifier.API.Accounts.Domain;
using Expensifier.API.Common.Queries;
using Marten.Schema;

namespace Expensifier.API.Accounts.GetById;

public record GetAccountByIdQuery(AccountId AccountId) : Query<Account>;