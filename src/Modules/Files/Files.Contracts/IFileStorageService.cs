using Application.Core.Results;
using Microsoft.AspNetCore.Http;

namespace Files.Contracts;

public interface IFileStorageService
{
    Task<Result<bool>> SaveFileAsync(IFormFile commandFile);
}