using Contract.Domain.ProgressAggregates;
using Contract.Helpers;
using Contract.Requests.Progress.RoutineRequests;
using Contract.Responses.Progress;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Progress.RoutineHandlers;

public sealed class CreateRoutineHandler(HealpathyContext context, IAppLogger logger, IEventCache cache)
    : RequestHandler<CreateRoutineCommand, RoutineModel, HealpathyContext>(context, logger, cache)
{
    public override async Task<Result<RoutineModel>> Handle(CreateRoutineCommand command, CancellationToken cancellationToken)
    {
        Routine entity = Adapt(command);

        try
        {
            await _context.Routines.InsertExt(entity);
            await _context.SaveChangesAsync(cancellationToken);

            _cache.Add(command.UserId, new Events.Routine_Created(entity.Id));
            return Created(RoutineModel.MapFunc(entity));
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private static Routine Adapt(CreateRoutineCommand command)
    {
        return new Routine(
            command.Id, command.UserId, command.Rq.Title, command.Rq.Description, command.Rq.Objective, command.Rq.Repeater,
            command.Rq.RepeaterSequenceId, command.Rq.StartDate.ToLocalTime(), command.Rq.EndDate.ToLocalTime(), command.Rq.IsCompleted, command.Rq.IsClosed, command.Rq.Tag);
    }
}