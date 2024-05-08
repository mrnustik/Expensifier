using MediatR;

namespace Expensifier.Common.Commands;

public abstract class CommandHandler<TCommand, TOutput> : IRequestHandler<TCommand, TOutput>
    where TCommand : Command<TOutput>
{
    protected abstract Task<TOutput> Execute(TCommand command, CancellationToken cancellationToken);
    
    public Task<TOutput> Handle(TCommand command, CancellationToken cancellationToken)
    {
        return Execute(command, cancellationToken);
    }
}