using Contract.Helpers;
using Contract.Requests.Courses.LectureCommentRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Course.LectureCommentHandlers;

public sealed class DeleteLectureCommentHandler : RequestHandler<DeleteLectureCommentCommand, HealpathyContext>
{
    public DeleteLectureCommentHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result> Handle(DeleteLectureCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.LectureComments.FindExt(request.Id);
        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != request.UserId)
            return Unauthorized(string.Empty);

        try
        {
            _context.LectureComments.SoftDeleteExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}