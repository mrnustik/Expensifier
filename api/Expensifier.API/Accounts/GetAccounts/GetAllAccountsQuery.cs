using Expensifier.API.Common.Queries;
using Expensifier.API.Common.Users;
using Marten;

namespace Expensifier.API.Accounts.GetAccounts;

public record GetAllAccountsQuery(UserId UserId)
    : Query<IReadOnlyCollection<AccountListItem>>
{
    public class Handler : QueryHandler<GetAllAccountsQuery, IReadOnlyCollection<AccountListItem>>
    {
        private readonly IDocumentSession _documentSession;

        public Handler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        protected override async Task<IReadOnlyCollection<AccountListItem>> Query(
            GetAllAccountsQuery query,
            CancellationToken cancellationToken)
        {
            return await _documentSession.Query<AccountListItem>()
                                         .Where(i => i.UserId == query.UserId.Value)
                                         .ToListAsync(cancellationToken);
        }
    }
}