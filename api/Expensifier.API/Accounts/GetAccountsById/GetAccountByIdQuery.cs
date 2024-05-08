using Expensifier.API.Accounts.Domain;
using Expensifier.API.Common.Queries;
using Marten;

namespace Expensifier.API.Accounts.GetAccountsById;

public record GetAccountByIdQuery(AccountId AccountId)
    : Query<AccountListInformation?>
{
    public class Handler : QueryHandler<GetAccountByIdQuery, AccountListInformation?>
    {
        private readonly IDocumentSession _documentSession;

        public Handler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        protected override async Task<AccountListInformation?> Query(GetAccountByIdQuery query,
                                                                     CancellationToken cancellationToken)
        {
            return await _documentSession.Query<AccountListInformation>()
                                         .SingleOrDefaultAsync(a => a.Id == query.AccountId.Value, cancellationToken);
        }
    }
}