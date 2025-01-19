using Contract.Helpers;
using Contract.Requests.Courses.CourseRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Courses.Services.Courses;

public sealed class DeleteCourseHandler : RequestHandler<DeleteCourseCommand, HealpathyContext>
{
    public DeleteCourseHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }

    public override async Task<Result> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Courses.FindExt(request.Id);
        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != request.UserId)
            return Unauthorized(string.Empty);

        try
        {
            _context.Courses.DeleteExt(entity);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    /*public override async Task<Result> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        bool result = await _context.DeleteOne<Course>(request.Id);

        return result ? Ok() : NotFound(string.Empty);
    }*/
}
