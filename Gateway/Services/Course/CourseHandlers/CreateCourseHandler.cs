using Contract.Domain.CourseAggregate;
using Contract.Helpers;
using Contract.Requests.Courses.CourseRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Courses.Services.Courses;

public sealed class CreateCourseHandler(HealpathyContext context, IAppLogger logger, IEventCache cache)
    : RequestHandler<CreateCourseCommand, HealpathyContext>(context, logger, cache)
{
    public override async Task<Result> Handle(CreateCourseCommand command, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FindExt(command.Rq.LeafCategoryId);
        if (category is null)
            return BadRequest(BusinessMessages.Course.INVALID_CATEGORY);

        try
        {
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

    private static Course Adapt(CreateCourseCommand command)
    {
        var lectures = (command.Rq.Lectures ?? []).Select(_ =>
            new Lecture(
                _.Id ?? Guid.NewGuid(), command.UserId,
                _.Title, _.Content, _.ContentSummary, _.IsPreviewable,
                _.Index, _.LectureType, _.MetaData,
                command.Id
            )
        ).ToList();

        return new Course(
            command.Id,
            command.UserId,
            command.InstructorId,
            command.Rq.LeafCategoryId,

            command.Rq.Title,
            command.Rq.Thumb.Url ?? string.Empty,
            command.Rq.Intro ?? string.Empty,
            command.Rq.Description ?? string.Empty,
            command.Rq.Status,

            command.Rq.Price ?? 0,
            command.Rq.Discount ?? 0,
            command.Rq.DiscountExpiry,

            command.Rq.Level,
            command.Rq.AdvisorExpectedOutcome ?? string.Empty,
            command.Rq.Outcomes ?? string.Empty,
            command.Rq.Requirements ?? string.Empty,
            command.Rq.ExpectedCompletion,
            
            lectures
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