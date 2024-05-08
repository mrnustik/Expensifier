namespace Expensifier.Common.Domain;

public abstract record Aggregate<TId>
{
    protected Aggregate(TId id)
    {
        Id = id;
    }

    public TId Id { get; }
}