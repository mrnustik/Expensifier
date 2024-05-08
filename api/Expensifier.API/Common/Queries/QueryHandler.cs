using MediatR;

namespace Expensifier.API.Common.Queries;

public abstract class QueryHandler<TQuery, TOutput> : IRequestHandler<TQuery, TOutput> 
    where TQuery : Query<TOutput>
{
    public Task<TOutput> Handle(TQuery request, CancellationToken cancellationToken)
    {
        return Query(request, cancellationToken);
    }
    
    protected abstract Task<TOutput> Query(TQuery query, CancellationToken cancellationToken);
}