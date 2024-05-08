using MediatR;

namespace Expensifier.API.Common.Queries;

public abstract record Query<TOutput> : IRequest<TOutput>;