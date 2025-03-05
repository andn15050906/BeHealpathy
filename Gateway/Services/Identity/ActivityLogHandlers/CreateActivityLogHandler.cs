using Contract.BusinessRules;
using Contract.Helpers;
using Contract.Requests.Identity.ActivityLogRequests;

namespace Gateway.Services.Identity.ActivityLogHandlers;

public sealed class CreateActivityLogHandler(HealpathyContext context, IAppLogger logger, IEventCache cache) : RequestHandler<CreateActivityLogCommand, HealpathyContext>(context, logger, cache)
{
    public override async Task<Result> Handle(CreateActivityLogCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _context.AddRangeAsync(
                request.Dtos.Select(_ => new ActivityLog(Guid.NewGuid(), request.UserId ?? PreSet.SystemUserId, _.CreationTime ?? DateTime.Now, _.Content))
            );
            await _context.SaveChangesAsync();
            return Created();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }
}