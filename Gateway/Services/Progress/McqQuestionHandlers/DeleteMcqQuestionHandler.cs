using Contract.Helpers;
using Contract.Requests.Progress.McqRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.McqQuestionHandlers;

public sealed class DeleteMcqQuestionHandler : RequestHandler<DeleteMcqQuestionCommand, HealpathyContext>
{
    public DeleteMcqQuestionHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result> Handle(DeleteMcqQuestionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.McqQuestions.FindExt(request.Id);
        if (entity is null)
            return NotFound(string.Empty);

        try
        {
            _context.McqQuestions.SoftDeleteExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}