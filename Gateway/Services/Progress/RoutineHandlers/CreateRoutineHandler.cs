using Contract.Domain.ProgressAggregates;
using Contract.Helpers;
using Contract.Requests.Progress.RoutineRequests;
using Contract.Responses.Progress;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.RoutineHandlers;

public sealed class CreateRoutineHandler : RequestHandler<CreateRoutineCommand, RoutineModel, HealpathyContext>
{
    public CreateRoutineHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<RoutineModel>> Handle(CreateRoutineCommand command, CancellationToken cancellationToken)
    {
        Routine entity = Adapt(command);

        try
        {
            await _context.Routines.InsertExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
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
            command.Rq.RepeaterSequenceId, command.Rq.StartDate, command.Rq.EndDate, command.Rq.IsCompleted, command.Rq.IsClosed, command.Rq.Tag);
    }
}