using Contract.Domain.ProgressAggregates;
using Contract.Helpers;
using Contract.Requests.Progress.RoutineLogRequests;
using Contract.Responses.Progress;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.RoutineLogHandlers;

public sealed class CreateRoutineLogHandler : RequestHandler<CreateRoutineLogCommand, RoutineLogModel, HealpathyContext>
{
    public CreateRoutineLogHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<RoutineLogModel>> Handle(CreateRoutineLogCommand command, CancellationToken cancellationToken)
    {
        RoutineLog entity = Adapt(command);

        try
        {
            await _context.RoutineLogs.InsertExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
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