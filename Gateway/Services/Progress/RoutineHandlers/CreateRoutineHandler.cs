using Contract.Domain.ProgressAggregates;
using Contract.Helpers;
using Contract.Requests.Progress.RoutineRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.RoutineHandlers;

public sealed class CreateRoutineHandler : RequestHandler<CreateRoutineCommand, HealpathyContext>
{
    public CreateRoutineHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result> Handle(CreateRoutineCommand command, CancellationToken cancellationToken)
    {
        Routine entity = Adapt(command);

        try
        {
            await _context.Routines.InsertExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Created();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private Routine Adapt(CreateRoutineCommand command)
    {
        return new Routine(command.Id, command.UserId, command.Rq.Title, command.Rq.Description, command.Rq.Frequency);
    }
}