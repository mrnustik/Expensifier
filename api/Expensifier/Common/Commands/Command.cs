using MediatR;

namespace Expensifier.Common.Commands;

public abstract record Command<TOutput> : IRequest<TOutput>
{
}