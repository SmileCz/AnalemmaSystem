using Application.Core.Results;
using Microsoft.AspNetCore.Http;

namespace Files.Contracts;

public interface IFileStorageService
{
    Task<Result<bool>> SaveFileAsync(IFormFile commandFile);
    Result<string> GetFilePath(string fileName);
    Result<IEnumerable<string>> GetAllFiles();
    Result<bool> DeleteFile(string fileName);
}