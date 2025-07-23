using Application.Core.CQRS;
using Application.Core.Results;
using Microsoft.AspNetCore.Http;

namespace Files.Application.Commands;

public record UploadFileCommand(IFormFile File) : ICommand<Result<bool>>;
