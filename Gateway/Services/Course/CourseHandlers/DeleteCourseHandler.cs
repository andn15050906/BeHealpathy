using Contract.Helpers;
using Contract.Requests.Courses.CourseRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Courses.Services.Courses;

/// <summary>
/// Handler xử lý việc xóa khóa học
/// </summary>
public sealed class DeleteCourseHandler : RequestHandler<DeleteCourseCommand, HealpathyContext>
{
    public DeleteCourseHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }

    /// <summary>
    /// Xử lý yêu cầu xóa khóa học
    /// </summary>
    public override async Task<Result> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        // Kiểm tra khóa học có tồn tại không
        var entity = await _context.Courses.FindExt(request.Id);
        if (entity is null)
            return NotFound(string.Empty);
        // Kiểm tra quyền xóa
        if (entity.CreatorId != request.UserId)
            return Unauthorized(string.Empty);

        try
        {
            // Thực hiện xóa khóa học
            _context.Courses.DeleteExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
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
