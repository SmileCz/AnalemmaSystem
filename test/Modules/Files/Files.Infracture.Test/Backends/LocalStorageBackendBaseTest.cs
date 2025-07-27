using System.IO;
using Files.Contracts;
using Files.Infrastructure.Backends;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Moq;

namespace Files.Infracture.Test.Backends;

public class LocalStorageBackendBaseTest : IDisposable
{
    protected readonly IStringLocalizer<LocalStorageBackendSaveFile> _localizer;
    protected readonly IStorageBackend _backend;

    private string Root { get; init; }

    protected LocalStorageBackendBaseTest()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        var provider = services.BuildServiceProvider();
        _localizer = provider.GetRequiredService<IStringLocalizer<LocalStorageBackendSaveFile>>();
        Root  = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory,"..", "..", "..", "..", "..", "..", "testData" + Guid.NewGuid()));
        _backend = new LocalStorageBackendSaveFile(Root,_localizer);

    }
    
   
    
    
    protected static IFormFile CreateMockFile(string filename, int length)
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns(filename);
        fileMock.Setup(f => f.Length).Returns(length);
        fileMock.Setup(f=> f.OpenReadStream()).Returns(new MemoryStream(new byte[length]));
        
        return fileMock.Object;
    }

    public void Dispose()
    {
        if (Directory.Exists(Root))
        {
            Directory.Delete(Root, true);
        }
    }
}