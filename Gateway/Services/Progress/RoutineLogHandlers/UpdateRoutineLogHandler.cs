using Contract.Domain.ProgressAggregates;
using Contract.Helpers;
using Contract.Requests.Progress.RoutineLogRequests;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Services.Library.RoutineLogHandlers;

public sealed class UpdateRoutineLogHandler : RequestHandler<UpdateRoutineLogCommand, HealpathyContext>
{
    public UpdateRoutineLogHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result> Handle(UpdateRoutineLogCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.RoutineLogs.FirstOrDefaultAsync(_ => _.Id == command.Rq.Id);

        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != command.UserId)
            return Unauthorized(string.Empty);

        try
        {
            entity = ApplyChanges(entity, command);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private RoutineLog ApplyChanges(RoutineLog entity, UpdateRoutineLogCommand command)
    {
        if (command.Rq.Content is not null)
            entity.Content = command.Rq.Content;
        return entity;
    }
}