using Contract.Domain.CourseAggregate;
using Contract.Helpers;
using Contract.Requests.Courses.CourseRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Courses.Services.Courses;

/// <summary>
/// Handler xử lý việc tạo mới khóa học
/// </summary>
public sealed class CreateCourseHandler(HealpathyContext context, IAppLogger logger, IEventCache cache)
    : RequestHandler<CreateCourseCommand, HealpathyContext>(context, logger, cache)
{
    /// <summary>
    /// Xử lý yêu cầu tạo mới khóa học
    /// </summary>
    public override async Task<Result> Handle(CreateCourseCommand command, CancellationToken cancellationToken)
    {
        // Kiểm tra danh mục khóa học có tồn tại không
        var category = await _context.Categories.FindExt(command.Rq.LeafCategoryId);
        if (category is null)
            return BadRequest(BusinessMessages.Course.INVALID_CATEGORY);

        try
        {
            // Chuyển đổi dữ liệu và lưu vào database
            var entity = Adapt(command);
            var courseTask = _context.Courses.InsertExt(entity);
            var mediaTask = _context.Multimedia.AddRangeAsync(command.Medias);
            await Task.WhenAll(courseTask, mediaTask);
            await _context.SaveChangesAsync(cancellationToken);

            _cache.Add(command.UserId, new Events.Course_Created(entity.Id));
            return Created();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    /// <summary>
    /// Chuyển đổi dữ liệu từ command sang entity Course
    /// </summary>
    private static Course Adapt(CreateCourseCommand command)
    {
        return new Course(
            command.Id, command.UserId, command.InstructorId, command.Rq.LeafCategoryId,
            command.Rq.Title, command.Rq.Thumb.Url ?? string.Empty, command.Rq.Intro, command.Rq.Description, command.Rq.Price,
            command.Rq.Level, command.Rq.Outcomes, command.Rq.Requirements/*, sections*/
        );
    }

    /*public override async Task<Result<Guid>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        try
        {
            //await _context.Insert(
            //    new Course(
            //        Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            //        "Web Development Bootcamp Title", "ThumbUrl", "Intro", "Description",
            //        100000, CourseLevel.All, "Outcomes", "Requirements", new() 
            //    )
            //);

            // instructorId

            var entity = Adapt(request);
            await _context.Insert(entity);


            return Created();
        }
        catch
        {
            return ServerError(string.Empty);
        }
    }*/
}