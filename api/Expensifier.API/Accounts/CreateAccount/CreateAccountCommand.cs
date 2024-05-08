using Expensifier.API.Accounts.Domain;
using Expensifier.API.Common.Commands;

namespace Expensifier.API.Accounts.CreateAccount;

public record CreateAccountCommand(
    string Name,
    decimal InitialBalance)
    : Command<AccountId>;