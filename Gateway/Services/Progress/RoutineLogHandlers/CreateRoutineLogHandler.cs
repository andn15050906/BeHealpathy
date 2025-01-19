using Contract.Domain.ProgressAggregates;
using Contract.Helpers;
using Contract.Requests.Progress.RoutineLogRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.RoutineLogHandlers;

public sealed class CreateRoutineLogHandler : RequestHandler<CreateRoutineLogCommand, HealpathyContext>
{
    public CreateRoutineLogHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result> Handle(CreateRoutineLogCommand command, CancellationToken cancellationToken)
    {
        RoutineLog entity = Adapt(command);

        try
        {
            await _context.RoutineLogs.InsertExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Created();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private RoutineLog Adapt(CreateRoutineLogCommand command)
    {
        return new RoutineLog(command.Id, command.UserId, command.Rq.RoutineId, command.Rq.Content);
    }
}