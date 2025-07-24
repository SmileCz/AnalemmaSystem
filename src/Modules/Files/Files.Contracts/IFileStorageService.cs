using Application.Core.Results;
using Microsoft.AspNetCore.Http;

namespace Files.Contracts;

public interface IFileStorageService
{
    Task<Result<bool>> SaveFileAsync(IFormFile file);
    Task<Result<bool>> SaveFileAsync(IFormFile file, string subFolder);
    
    Result<string> GetFilePath(string fileName);
    Result<string> GetFilePath(string fileName,string subFolder);
    
    Result<IEnumerable<string>> ListFiles();
    Result<IEnumerable<string>> ListFiles(string subFolder);
    
    Result<bool> DeleteFile(string fileName);
    Result<bool> DeleteFile(string fileName,string subFolder);
    
    Result<bool> MoveFile(string fileName, string destinationFolder);
    
}