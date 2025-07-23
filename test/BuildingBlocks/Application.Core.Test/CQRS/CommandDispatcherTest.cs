namespace Application.Core.CQRS;

using Moq;

public class CommandDispatcherTest
{

    [Fact]
    public async Task SendShouldReturnsResult()
    {

        var handler = new TestCommandHandler();
        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock
            .Setup(sp => sp.GetService(typeof(ICommandHandler<TestCommand, string>)))
            .Returns(handler);
        
        var dispatcher = new CommandDispatcher(serviceProviderMock.Object);
        var result = await dispatcher.Send(new TestCommand(), CancellationToken.None);
        
        Assert.Equal("OK", result);
    }
    
}