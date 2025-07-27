using Files.Contracts;
using Files.Infrastructure.Backends;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Moq;

namespace Files.Infracture.Test.Backends;

[TestSubject(typeof(LocalStorageBackendSaveFile))]
public class LocalStorageBackendSaveFileTest : LocalStorageBackendBaseTest
{
    [Fact]
    public void SaveFileAsync_ShouldSaveFile_WhenValidFileProvided()
    {
        var fileMock = CreateMockFile("testfile.txt", 100);
        var result = _backend.SaveFileAsync(fileMock, "testfolder").Result; 
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public void SaveFileAsync_ShouldReturnFailure_WhenFileIsEmpty()
    {
        var fileMock = CreateMockFile("emptyfile.txt", 0);
        var result = _backend.SaveFileAsync(fileMock, "testfolder").Result;
        Assert.True(result.IsFailure);
        Assert.Equal(_localizer["SourceFileEmpty"].Value, result.Error);
    }
    
    [Fact]
    public void SaveFileAsync_ShouldReturnFailure_WhenFileTooLarge()
    {
        var fileMock = CreateMockFile("largefile.txt", 11_000_000); // 11 MB
        var result = _backend.SaveFileAsync(fileMock, "testfolder").Result;
        Assert.True(result.IsFailure);
        Assert.Equal(_localizer["SourceFileTooLarge"].Value, result.Error);
    }

    [Fact]
    public void SaveFileAsync_ShouldNotSaveFile_WhenInValidFileProvided()
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("invalidfile.txt");
        fileMock.Setup(f => f.Length).Returns(100);
        fileMock.Setup(f => f.OpenReadStream()).Throws(new IOException("File not found"));
        
        var result = _backend.SaveFileAsync(fileMock.Object, "testfolder").Result;
        Assert.True(result.IsFailure);
        Assert.Equal(_localizer["FileSaveError", "File not found"].Value, result.Error);
    }
    
}