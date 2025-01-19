using Contract.Helpers;
using Contract.Requests.Library.MediaRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.MediaResourceHandlers;

public sealed class DeleteMediaResourceHandler : RequestHandler<DeleteMediaResourceCommand, HealpathyContext>
{
    public DeleteMediaResourceHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result> Handle(DeleteMediaResourceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.MediaResources.FindExt(request.Id);
        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != request.UserId)
            return Unauthorized(string.Empty);

        try
        {
            _context.MediaResources.SoftDeleteExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}