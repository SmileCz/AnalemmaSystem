namespace Application.Core.CQRS;

public class TestCommandHandler : ICommandHandler<TestCommand, string>
{
    public Task<string> Handle(TestCommand command, CancellationToken cancellationToken)
    {
        return Task.FromResult("OK");
    }
}