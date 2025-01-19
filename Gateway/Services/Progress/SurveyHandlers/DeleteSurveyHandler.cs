using Contract.Helpers;
using Contract.Requests.Progress.SurveyRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.SurveyHandlers;

public sealed class DeleteSurveyHandler : RequestHandler<DeleteSurveyCommand, HealpathyContext>
{
    public DeleteSurveyHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result> Handle(DeleteSurveyCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Surveys.FindExt(request.Id);
        if (entity is null)
            return NotFound(string.Empty);

        try
        {
            _context.Surveys.SoftDeleteExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}