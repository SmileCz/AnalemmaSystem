namespace Files.Infracture.Test.Backends;

public class LocalStorageBackendMoveFileTest : LocalStorageBackendBaseTest
{
    [Fact]
    public async Task MoveFileAsync_WhenSourceExists_MovesFileToDestination()
    {
        // Arrange
        const string sourcePath = "testfolder/sourcefile.txt";
        const string destinationPath = "testfolder/destinationfile.txt";
        await _backend.SaveFileAsync(CreateMockFile("sourcefile.txt", 100), "testfolder");
        var result = _backend.MoveFile(sourcePath, destinationPath);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task MoveFileAsync_WhenSourceDoesNotExist_ThenShouldReturnError()
    {
        const string sourcePath = "testfolder/nonexistentfile.txt";
        const string destinationPath = "testfolder/destinationfile.txt";
        var result = _backend.MoveFile(sourcePath, destinationPath);
        Assert.False(result.IsSuccess);
        Assert.Equal(_localizer["SourceFileNotExist", sourcePath], result.ErrorOrDefault);
    }

    [Fact]
    public async Task MoveFileAsync_WhenDestinationCannotBeCreated_ThenShouldReturnError()
    {
        const string sourcePath = "testfolder/sourcefile.txt";
        const string destinationPath = "testfolder/inv\0alid/destinationfile.txt"; // Invalid path
        await _backend.SaveFileAsync(CreateMockFile("sourcefile.txt", 100), "testfolder");
        var result = _backend.MoveFile(sourcePath, destinationPath);
        Assert.False(result.IsSuccess);
        Assert.Contains(_localizer["FileMoveError", ""].Value, result.ErrorOrDefault);
    }
}