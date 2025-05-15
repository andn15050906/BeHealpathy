using Contract.Helpers;
using Contract.Requests.Courses.CourseRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Course.EnrollmentHandlers;

public sealed class DeleteEnrollmentHandler(HealpathyContext context, IAppLogger logger, IEventCache cache)
    : RequestHandler<DeleteCourseCommand, HealpathyContext>(context, logger, cache)
{
    public override async Task<Result> Handle(DeleteCourseCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.CourseProgress.FindExt(command.Id);
        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != command.UserId)
            return Unauthorized(string.Empty);

        try
        {
            _context.CourseProgress.DeleteExt(entity);
            _cache.Add(command.UserId, new Events.Course_Unenrolled(command.Id));
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}
