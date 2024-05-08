namespace Expensifier.API.Accounts.Domain;

public class Account
{
    public Account()
    {
    }

    public Account(AccountCreated started)
    {
        Id = started.Id.Value;
        Name = started.Name;
        Balance = 0;
    }

    public Guid Id { get; set; }
    public string Name { get; }
    public decimal Balance { get; }
}