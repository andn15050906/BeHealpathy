using Contract.Helpers;
using Contract.Requests.Progress.RoutineRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Progress.RoutineHandlers;

public sealed class DeleteRoutineHandler : RequestHandler<DeleteRoutineCommand, HealpathyContext>
{
    public DeleteRoutineHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result> Handle(DeleteRoutineCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Routines.FindExt(request.Id);
        if (entity is null)
            return NotFound(string.Empty);

        try
        {
            _context.Routines.SoftDeleteExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}