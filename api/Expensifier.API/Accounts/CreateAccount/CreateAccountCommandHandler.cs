using Expensifier.API.Accounts.Domain;
using Expensifier.API.Common.Commands;
using Marten;

namespace Expensifier.API.Accounts.CreateAccount;

public class CreateAccountCommandHandler : CommandHandler<CreateAccountCommand, AccountId>
{
    private readonly IDocumentSession _documentSession;

    public CreateAccountCommandHandler(IDocumentSession documentSession)
    {
        _documentSession = documentSession;
    }

    protected override async Task<AccountId> Execute(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var accountId = AccountId.New();

        var createdEvent = new AccountCreated(accountId, command.Name, command.InitialBalance);
        _documentSession.Events.Append(accountId.Value,
                                       createdEvent);
        await _documentSession.SaveChangesAsync(cancellationToken);

        return accountId;
    }
}