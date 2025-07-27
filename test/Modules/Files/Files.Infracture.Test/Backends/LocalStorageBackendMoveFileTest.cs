namespace Files.Infracture.Test.Backends;

public class LocalStorageBackendMoveFileTest : LocalStorageBackendBaseTest
{


    [Fact]
    public async Task MoveFileAsync_WhenSourceExists_MovesFileToDestination()
    {
        // Arrange
        const string sourcePath = "testfolder/sourcefile.txt";
        const string destinationPath = "testfolder/destinationfile.txt";
        await _backend.SaveFileAsync(CreateMockFile("sourcefile.txt", 100), "moveTestFolder");

        var result = _backend.MoveFile(sourcePath, destinationPath);
        Assert.True(result.IsSuccess);
    }
}