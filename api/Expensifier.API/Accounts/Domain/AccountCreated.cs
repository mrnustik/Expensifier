using Expensifier.API.Common.Users;

namespace Expensifier.API.Accounts.Domain;

public record AccountCreated(
    AccountId Id,
    string Name,
    UserId UserId);