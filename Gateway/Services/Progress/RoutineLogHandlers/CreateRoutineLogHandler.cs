using Contract.Domain.ProgressAggregates;
using Contract.Helpers;
using Contract.Requests.Progress.RoutineLogRequests;
using Contract.Responses.Progress;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Progress.RoutineLogHandlers;

public sealed class CreateRoutineLogHandler(HealpathyContext context, IAppLogger logger, IEventCache cache)
    : RequestHandler<CreateRoutineLogCommand, RoutineLogModel, HealpathyContext>(context, logger, cache)
{
    public override async Task<Result<RoutineLogModel>> Handle(CreateRoutineLogCommand command, CancellationToken cancellationToken)
    {
        RoutineLog entity = Adapt(command);

        try
        {
            await _context.RoutineLogs.InsertExt(entity);
            await _context.SaveChangesAsync(cancellationToken);

            _cache.Add(command.UserId, new Events.RoutineLog_Created(entity.Id));
            return Created(RoutineLogModel.MapFunc(entity));
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private static RoutineLog Adapt(CreateRoutineLogCommand command)
    {
        return new RoutineLog(command.Id, command.UserId, command.Rq.RoutineId, command.Rq.Content);
    }
}