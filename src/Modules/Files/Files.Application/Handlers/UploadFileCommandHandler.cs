using Application.Core.CQRS;
using Application.Core.Results;
using Files.Application.Commands;

namespace Files.Application.Handlers;

public class UploadFileCommandHandler : ICommandHandler<UploadFileCommand, Result<bool>>
{
    
    
    public Task<Result<bool>> Handle(UploadFileCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}