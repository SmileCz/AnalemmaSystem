using Application.Core.Results;
using Microsoft.AspNetCore.Http;

namespace Files.Contracts;

public interface IStorageBackend
{
    Task<Result<bool>> SaveFileAsync(IFormFile file,string relativePath);
    Result<bool> DeleteFile(string relativePath);
    Result<IEnumerable<string>> ListFiles(string relativePath);
    Result<string> GetAbsoluteFilePath(string relativePath);
    Result<bool> MoveFile(string sourcePath, string destinationPath);
    
}