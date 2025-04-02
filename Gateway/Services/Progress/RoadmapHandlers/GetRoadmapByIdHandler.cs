using Contract.Helpers;
using Contract.Requests.Progress.RoadmapRequests.Dtos;
using Contract.Responses.Progress;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Progress.RoadmapHandlers
{
    public class GetRoadmapByIdHandler(HealpathyContext context, IAppLogger logger)
    : RequestHandler<GetRoadmapByIdQuery, RoadmapModel?, HealpathyContext>(context, logger)
    {
        public override async Task<Result<RoadmapModel?>> Handle(GetRoadmapByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var roadmap = await _context.Roadmaps
                    .Where(r => r.Id == request.Id && !r.IsDeleted)
                    .Select(RoadmapModel.MapExpression)
                    .FirstOrDefaultAsync(cancellationToken);

                return ToQueryResult(roadmap);
            }
            catch (Exception ex)
            {
                return ServerError(ex.Message);
            }
        }
    }
}
