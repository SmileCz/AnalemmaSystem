using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Application.Core.CQRS;

public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private static readonly ConcurrentDictionary<Type, Func<object, object, CancellationToken, object>> _handleDelegates = new();

    public Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        var commandType = command.GetType();
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResponse));
        var handler = _serviceProvider.GetService(handlerType) ?? throw new InvalidOperationException($"Handler not found for command type {commandType.Namespace}.{commandType.Name}");


        var handleDelegate = _handleDelegates.GetOrAdd(handlerType, ht =>
        {
            var method = ht.GetMethod("Handle");
            var commandParameter = Expression.Parameter(typeof(object),"command");
            var handlerParameter = Expression.Parameter(typeof(object),"handler");
            var cancellationTokenParameter = Expression.Parameter(typeof(CancellationToken),"cancellationToken");

            var call = Expression.Call(
                Expression.Convert(handlerParameter, ht),
                method,
                Expression.Convert(commandParameter, commandType),
                cancellationTokenParameter);

            var lamdba = Expression.Lambda<Func<object,object,CancellationToken,object>>(call,handlerParameter,commandParameter,cancellationTokenParameter);
            return lamdba.Compile();
        });
        var result = handleDelegate(handler,command,cancellationToken);
        return (Task<TResponse>)result;
    }
}    