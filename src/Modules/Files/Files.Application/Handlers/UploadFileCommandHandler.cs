using Application.Core.CQRS;
using Application.Core.Results;
using Files.Application.Commands;
using Files.Contracts;

namespace Files.Application.Handlers;

public class UploadFileCommandHandler : ICommandHandler<UploadFileCommand, Result<bool>>
{
    private readonly IFileStorageService _fileStorageService;
    
    
    public async Task<Result<bool>> Handle(UploadFileCommand command, CancellationToken cancellationToken)
    {
        return await _fileStorageService.SaveFileAsync(command.File);
            
    }
}