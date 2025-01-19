using Contract.Helpers;
using Contract.Requests.Courses.LectureRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Course.LectureHandlers;

public sealed class DeleteLectureHandler : RequestHandler<DeleteLectureCommand, HealpathyContext>
{
    public DeleteLectureHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }

    public override async Task<Result> Handle(DeleteLectureCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Lectures.FindExt(request.Id);
        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != request.UserId)
            return Unauthorized(string.Empty);

        try
        {
            _context.Lectures.DeleteExt(entity);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}