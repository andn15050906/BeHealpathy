using Contract.Helpers;
using Contract.Requests.Library.TagRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.TagHandlers;

public class DeleteTagHandler : RequestHandler<DeleteTagCommand, HealpathyContext>
{
    public DeleteTagHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Tags.FindExt(request.Id);
        if (entity is null)
            return NotFound(string.Empty);

        try
        {
            _context.Tags.SoftDeleteExt(entity);
            _context.SaveChanges();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }
}