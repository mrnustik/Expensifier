namespace Expensifier.API.Accounts.Domain;

public record AccountCreated(
    AccountId Id,
    string Name);