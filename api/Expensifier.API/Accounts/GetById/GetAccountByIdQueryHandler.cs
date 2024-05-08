using Expensifier.API.Accounts.Domain;
using Expensifier.API.Common.Queries;
using Marten;

namespace Expensifier.API.Accounts.GetById;

public class GetAccountByIdQueryHandler : QueryHandler<GetAccountByIdQuery, Account>
{
    private readonly IDocumentSession _documentSession;

    public GetAccountByIdQueryHandler(IDocumentSession documentSession)
    {
        _documentSession = documentSession;
    }

    protected override async Task<Account> Query(GetAccountByIdQuery query, CancellationToken cancellationToken)
    {
        var account = await _documentSession.Events.AggregateStreamAsync<Account>(query.AccountId.Value, token: cancellationToken);
        return account!;
    }
}