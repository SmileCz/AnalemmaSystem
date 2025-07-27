using Application.Core.Results;
using Files.Contracts;
using Microsoft.AspNetCore.Http;

namespace Files.Infrastructure;

public class FileStorageService(IStorageBackend backend) : IFileStorageService
{
    public async Task<Result<bool>> SaveFileAsync(IFormFile file)
    {
        return await SaveFileAsync(file, "");
    }

    public async Task<Result<bool>> SaveFileAsync(IFormFile file, string subFolder)
    {
        var path = Path.Combine(subFolder, file.FileName);
        return await backend.SaveFileAsync(file, path);
    }

    public Result<string> GetFilePath(string fileName)
    {
        return GetFilePath(fileName, "");
    }

    public Result<string> GetFilePath(string fileName, string subFolder)
    {
        var path = Path.Combine(subFolder, fileName);
        return backend.GetAbsoluteFilePath(path);
    }

    public async Task<Result<IEnumerable<string>>> ListFiles()
    {
        return await ListFiles("");
    }

    public async Task<Result<IEnumerable<string>>> ListFiles(string subFolder)
    {
        return await backend.ListFilesAsync(subFolder);
    }

    public Result<bool> DeleteFile(string fileName)
    {
        return DeleteFile(fileName, "");
    }

    public Result<bool> DeleteFile(string fileName, string subFolder)
    {
        var path = Path.Combine(subFolder, fileName);
        return backend.DeleteFile(path);
    }

    public Result<bool> MoveFile(string fileName, string destinationFolder)
    {
        return backend.MoveFile(fileName, destinationFolder);
    }
}