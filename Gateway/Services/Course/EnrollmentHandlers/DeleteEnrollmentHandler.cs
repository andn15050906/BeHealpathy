using Contract.Helpers;
using Contract.Requests.Courses.CourseRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Course.EnrollmentHandlers;

public sealed class DeleteEnrollmentHandler : RequestHandler<DeleteCourseCommand, HealpathyContext>
{
    public DeleteEnrollmentHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }

    public override async Task<Result> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Enrollments.FindExt(request.Id);
        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != request.UserId)
            return Unauthorized(string.Empty);

        try
        {
            _context.Enrollments.DeleteExt(entity);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}
