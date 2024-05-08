using MediatR;

namespace Expensifier.API.Common.Commands;

public abstract record Command<TOutput> : IRequest<TOutput>
{
}