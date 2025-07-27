using JetBrains.Annotations;

namespace Files.Infracture.Test.Backends;

[TestSubject(typeof(LocalStorageBackendDeleteFileTest))]
public class LocalStorageBackendDeleteFileTest : LocalStorageBackendBaseTest
{

    [Fact]
    public async Task DeleteFile_ShouldDeleteFile_WhenFileExists()
    {
        const string filePath = "testfolder/testfile.txt";
        await _backend.SaveFileAsync(CreateMockFile("testfile.txt", 100), "testfolder");
        var result = _backend.DeleteFile(filePath);
        Assert.True(result.IsSuccess);
        
        
    }

    [Fact]
    public void DeleteFile_ShouldReturnFailure_WhenFileDoesNotExist()
    {
        const string filePath = "nonexistentfolder/nonexistentfile.txt";
        var result = _backend.DeleteFile(filePath);
        Assert.False(result.IsSuccess);
        Assert.Equal(_localizer["SourceFileNotExist", filePath], result.ErrorOrDefault);
    }



}