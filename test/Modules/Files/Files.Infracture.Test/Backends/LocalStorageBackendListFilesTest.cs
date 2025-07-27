using Files.Contracts;
using JetBrains.Annotations;

namespace Files.Infracture.Test.Backends;

[TestSubject(typeof(LocalStorageBackendListFilesTest))]
public class LocalStorageBackendListFilesTest : LocalStorageBackendBaseTest
{
    
    [Fact]
    public async Task ListFiles_ShouldReturnFiles_WhenDirectoryExists()
    {
        const string directory = "listfilesfolder";
        await _backend.SaveFileAsync(CreateMockFile("file1.txt", 100), directory);
        await _backend.SaveFileAsync(CreateMockFile("file2.txt", 200), directory);

        var result = await _backend.ListFilesAsync(directory);
        
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.ToList().Count);
        Assert.Contains(result.Value.ToList(), f => f == "file1.txt");
        Assert.Contains(result.Value.ToList(), f => f == "file2.txt");
    }

    [Fact]
    public async Task ListFiles_ShouldReturnEmptyList_WhenDirectoryDoesNotExist()
    {
        const string directory = "nonexistentfolder";
        
        var result = await _backend.ListFilesAsync(directory);
        
        Assert.True(result.IsFailure);
        
    }
    
}