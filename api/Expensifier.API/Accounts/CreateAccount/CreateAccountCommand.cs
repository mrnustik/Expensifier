using Expensifier.API.Accounts.Domain;
using Expensifier.API.Common.Commands;
using Expensifier.API.Common.Users;
using Marten;

namespace Expensifier.API.Accounts.CreateAccount;

public record CreateAccountCommand(
    string Name,
    UserId UserId)
    : Command<AccountId>
{
    public class Handler : CommandHandler<CreateAccountCommand, AccountId>
    {
        private readonly IDocumentSession _documentSession;

        public Handler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        protected override async Task<AccountId> Execute(CreateAccountCommand command,
                                                         CancellationToken cancellationToken)
        {
            var accountId = AccountId.New();

            var createdEvent = new AccountCreated(accountId, command.Name, command.UserId);
            _documentSession.Events.Append(accountId.Value,
                                           createdEvent);
            await _documentSession.SaveChangesAsync(cancellationToken);

            return accountId;
        }
    }
}