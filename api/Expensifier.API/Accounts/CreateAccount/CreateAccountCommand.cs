﻿using Expensifier.API.Accounts.Domain;
using Expensifier.API.Common.Commands;
using Expensifier.API.Common.Users;
using Marten;

namespace Expensifier.API.Accounts.CreateAccount;

public record CreateAccountCommand(
    string Name)
    : Command<AccountId>
{
    public class Handler : CommandHandler<CreateAccountCommand, AccountId>
    {
        private readonly IDocumentSession _documentSession;
        private readonly IUserProvider _userProvider;

        public Handler(IDocumentSession documentSession, IUserProvider userProvider)
        {
            _documentSession = documentSession;
            _userProvider = userProvider;
        }

        protected override async Task<AccountId> Execute(CreateAccountCommand command,
                                                         CancellationToken cancellationToken)
        {
            var accountId = AccountId.New();

            var createdEvent = new AccountCreated(accountId, command.Name, _userProvider.CurrentUserId);
            _documentSession.Events.Append(accountId.Value,
                                           createdEvent);
            await _documentSession.SaveChangesAsync(cancellationToken);

            return accountId;
        }
    }
}