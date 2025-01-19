using Contract.Helpers;
using Contract.Requests.Library.TagRequests;
using Contract.Responses.Library;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Library.TagHandlers;

public sealed class GetPagedTagsHandler : RequestHandler<GetAllTagsQuery, List<TagModel>, HealpathyContext>
{
    public GetPagedTagsHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<List<TagModel>>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _context.Tags
                .Where(_ => !_.IsDeleted)
                .Select(TagModel.MapExpression)
                .ToListAsync();
            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }
}