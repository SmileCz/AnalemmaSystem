using Application.Core.Results;
using Files.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Files.Infrastructure.Backends;

public class LocalStorageBackendSaveFile
    : IStorageBackend
{
    private readonly string _root;
    private readonly IStringLocalizer<LocalStorageBackendSaveFile> _localizer;

    public LocalStorageBackendSaveFile(string? root = null,
        IStringLocalizer<LocalStorageBackendSaveFile> localizer = null)
    {
        _root = root ?? "incoming";
        _localizer = localizer;
        Directory.CreateDirectory(_root);
    }

    private string ResolvePath(string? relativePath) =>
        string.IsNullOrWhiteSpace(relativePath)
            ? _root
            : Path.Combine(_root, relativePath.Replace('/', Path.DirectorySeparatorChar));


    public async Task<Result<bool>> SaveFileAsync(IFormFile file, string? relativePath)
    {
        try
        {
            // Pokud relativePath neobsahuje jméno souboru, přidej jej
            var path = string.IsNullOrWhiteSpace(relativePath) || !Path.HasExtension(relativePath)
                ? Path.Combine(relativePath ?? string.Empty, file.FileName)
                : relativePath;

            var fullPath = Path.GetFullPath(ResolvePath(path));
            var directory = Path.GetDirectoryName(fullPath);
            Directory.CreateDirectory(directory);
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
                    await fs.FlushAsync();
                    return Result<bool>.Success(true);
            }
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(_localizer["FileSaveError", ex.Message]);
        }
    }

    public Result<bool> DeleteFile(string? relativePath)
    {
        try
        {
            var fullPath = Path.GetFullPath(ResolvePath(relativePath));
            if (!File.Exists(fullPath)) return Result<bool>.Failure(_localizer["SourceFileNotExist"]);
            File.Delete(fullPath);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(_localizer["FileDeleteError", ex.Message]);
        }
    }

    public async Task<Result<IEnumerable<string>>> ListFilesAsync(string relativePath)
    {
        var directory = ResolvePath(relativePath);
        if (!Directory.Exists(directory))
            return Result<IEnumerable<string>>.Failure(_localizer["DirectoryNotExist", relativePath]);
        var files = await Task.Run(() => Directory.GetFiles(directory)).ConfigureAwait(false);
        var fileNames = files.Select(Path.GetFileName);
        return Result<IEnumerable<string>>.Success(fileNames);
    }

    public Result<string> GetAbsoluteFilePath(string relativePath)
    {
        var fullPath = Path.GetFullPath(ResolvePath(relativePath));
        return !File.Exists(fullPath)
            ? Result<string>.Failure(_localizer["SourceFileNotExist"])
            : Result<string>.Success(fullPath);
    }

    public Result<bool> MoveFile(string from, string to)
    {
        var fromFullPath = ResolvePath(from);
        var toFullPath = ResolvePath(to);

        if (!File.Exists(fromFullPath))
            return Result<bool>.Failure(_localizer["SourceFileNotExist"]);

        try
        {
            var directory = Path.GetDirectoryName(toFullPath);
            if (directory != null) Directory.CreateDirectory(directory);
            File.Move(fromFullPath, toFullPath, true);
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(_localizer["FileMoveError", ex.Message]);
        }
    }
}