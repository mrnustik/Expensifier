using Expensifier.API.Accounts.Domain;
using Expensifier.API.Common.Queries;
using Marten;

namespace Expensifier.API.Accounts.GetAccountById;

public record GetAccountByIdQuery(AccountId AccountId)
    : Query<AccountDetail?>
{
    public class Handler : QueryHandler<GetAccountByIdQuery, AccountDetail?>
    {
        private readonly IDocumentSession _documentSession;

        public Handler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        protected override async Task<AccountDetail?> Query(GetAccountByIdQuery query,
                                                            CancellationToken cancellationToken)
        {
            return await _documentSession.Query<AccountDetail>()
                                         .SingleOrDefaultAsync(a => a.Id == query.AccountId.Value, cancellationToken);
        }
    }
}