using Contract.Helpers;
using Contract.Requests.Progress.SubmissionRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Progress.SubmissionHandlers;

public sealed class DeleteMcqQuestionHandler : RequestHandler<DeleteSubmissionCommand, HealpathyContext>
{
    public DeleteMcqQuestionHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result> Handle(DeleteSubmissionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Submissions.FindExt(request.Id);
        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != request.UserId)
            return Unauthorized(string.Empty);

        try
        {
            _context.Submissions.SoftDeleteExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}