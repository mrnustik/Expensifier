using MediatR;

namespace Expensifier.Common.Queries;

public abstract record Query<TOutput> : IRequest<TOutput>;