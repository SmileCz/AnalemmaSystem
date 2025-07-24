using Application.Core.Results;
using Files.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Files.Infrastructure.Backends;

public class LocalStorageBackend(string root, IStringLocalizer<LocalStorageBackend> localizer)
    : IStorageBackend
{
    private readonly IStringLocalizer<LocalStorageBackend> _localizer = localizer;

    private string ResolvePath(string relativePath) =>
        string.IsNullOrWhiteSpace(relativePath) ? 
            root : 
            Path.Combine(root, relativePath.Replace('/', Path.DirectorySeparatorChar));
    
    
    public async Task<Result<bool>> SaveFileAsync(IFormFile file, string relativePath)
    {
        try
        {
            var fullPath = Path.GetFullPath(ResolvePath(relativePath));
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
            await using var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 81920,
                true);
            await using var input = file.OpenReadStream();
            switch (input.Length)
            {
                case 0:
                    return Result<bool>.Failure(_localizer["SourceFileEmpty"]);
                // 10 MB limit
                case > 10_000_000:
                    return Result<bool>.Failure(_localizer["SourceFileTooLarge"]);
                default:
                    await input.CopyToAsync(fs);
                    return Result<bool>.Success(true);
            }
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(_localizer["FileSaveError", ex.Message]);
        }
    }

    public Result<bool> DeleteFile(string relativePath)
    {
        throw new NotImplementedException();
    }

    public Result<IEnumerable<string>> ListFiles(string relativePath)
    {
        throw new NotImplementedException();
    }

    public Result<string> GetAbsoluteFilePath(string relativePath)
    {
        throw new NotImplementedException();
    }

    public Result<bool> MoveFile(string sourcePath, string destinationPath)
    {
        throw new NotImplementedException();
    }
}