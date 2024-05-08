namespace Expensifier.API.Accounts.Domain;

public class Account
{  
    public Guid Id { get; set; }
    public string Name { get; }
    public decimal Balance { get; }

    public Account()
    {
    }
    
    public Account(AccountCreated started)
    {
        Id = started.Id.Value;
        Name = started.Name;
        Balance = started.InitialBalance;
    }
}

public record AccountCreated(
    AccountId Id,
    string Name, 
    decimal InitialBalance)
{
}