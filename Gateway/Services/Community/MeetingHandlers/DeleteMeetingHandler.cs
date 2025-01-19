using Contract.Helpers;
using Contract.Requests.Community.MeetingRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Community.MeetingHandlers;

public sealed class DeleteMeetingHandler : RequestHandler<DeleteMeetingCommand, HealpathyContext>
{
    public DeleteMeetingHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result> Handle(DeleteMeetingCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Meetings.FindExt(request.Id);
        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != request.UserId)
            return Unauthorized(string.Empty);

        try
        {
            _context.Meetings.SoftDeleteExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}