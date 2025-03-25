using Contract.Helpers;
using Contract.Requests.Progress.RoadmapRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Progress.RoadmapHandlers;

public class DeleteRoadmapHandler(HealpathyContext context, IAppLogger logger)
    : RequestHandler<DeleteRoadmapCommand, HealpathyContext>(context, logger)
{
    public override async Task<Result> Handle(DeleteRoadmapCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Roadmaps.FindExt(request.Id);
        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != request.UserId)
            return Unauthorized(string.Empty);

        try
        {
            _context.Roadmaps.SoftDeleteExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}