namespace Application.Core.CQRS;

public interface ICommandDispatcher
{
    Task<TResponse>  Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default);
}