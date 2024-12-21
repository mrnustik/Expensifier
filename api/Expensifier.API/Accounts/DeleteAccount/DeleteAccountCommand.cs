using Expensifier.API.Accounts.Domain;
using Expensifier.API.Common.Commands;
using Marten;

namespace Expensifier.API.Accounts.DeleteAccount;

public record DeleteAccountCommand(
    AccountId AccountId) : Command<AccountId>
{
    public class Handler : CommandHandler<DeleteAccountCommand, AccountId>
    {
        private readonly IDocumentSession _documentSession;

        public Handler(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        protected override async Task<AccountId> Execute(DeleteAccountCommand command, CancellationToken cancellationToken)
        {
            _documentSession.Events
                            .Append(command.AccountId.Value, new AccountDeleted());
            await _documentSession.SaveChangesAsync(cancellationToken);
            return command.AccountId;
        }
    }
}