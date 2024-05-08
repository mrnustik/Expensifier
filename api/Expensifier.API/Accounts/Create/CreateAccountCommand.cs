using Expensifier.API.Accounts.Domain;
using Expensifier.API.Common.Commands;

namespace Expensifier.API.Accounts.Create;

public record CreateAccountCommand(
    string Name,
    decimal InitialBalance)
    : Command<AccountId>;