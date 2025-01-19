using Contract.Helpers;
using Contract.Requests.Progress.RoutineLogRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.RoutineLogHandlers;

public sealed class DeleteRoutineLogHandler : RequestHandler<DeleteRoutineLogCommand, HealpathyContext>
{
    public DeleteRoutineLogHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result> Handle(DeleteRoutineLogCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.RoutineLogs.FindExt(request.Id);
        if (entity is null)
            return NotFound(string.Empty);

        try
        {
            _context.RoutineLogs.SoftDeleteExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}