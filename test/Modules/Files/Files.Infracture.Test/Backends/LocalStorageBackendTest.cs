using Files.Infrastructure.Backends;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Moq;

namespace Files.Infracture.Test.Backends;

[TestSubject(typeof(LocalStorageBackend))]
public class LocalStorageBackendTest
{

    private readonly IStringLocalizer<LocalStorageBackend> _localizer;

    public LocalStorageBackendTest()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        var provider = services.BuildServiceProvider();
        _localizer = provider.GetRequiredService<IStringLocalizer<LocalStorageBackend>>();
    }
    
    [Fact]
    public void SaveFileAsync_ShouldSaveFile_WhenValidFileProvided()
    {
        // Arrange
        var backend = new LocalStorageBackend("",_localizer);
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("testfile.txt");
        fileMock.Setup(f => f.Length).Returns(100);
        fileMock.Setup(f=> f.OpenReadStream()).Returns(new MemoryStream(new byte[100]));
        
        // Act
        var result = backend.SaveFileAsync(fileMock.Object, "testfolder").Result;

        // Assert
        Assert.True(result.IsSuccess);
    }
}