using Contract.Domain.CourseAggregate;
using Contract.Helpers;
using Contract.Requests.Courses.LectureRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Course.LectureHandlers;

public sealed class CreateLectureHandler : RequestHandler<CreateLectureCommand, HealpathyContext>
{
    public CreateLectureHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }

    public override async Task<Result> Handle(CreateLectureCommand command, CancellationToken cancellationToken)
    {
        var entity = Adapt(command, command.UserId);

        try
        {
            var lectureTask = _context.Lectures.InsertExt(entity);
            var mediaTask = command.Medias is not null
                ? _context.Multimedia.AddRangeAsync(command.Medias.Where(_ => _ is not null))
                : Task.CompletedTask;
            await Task.WhenAll(lectureTask, mediaTask);
            await _context.SaveChangesAsync(cancellationToken);

            await _context.Lectures.InsertExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Created();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Lecture Adapt(CreateLectureCommand command, Guid userId)
    {
        return new Lecture(command.Id, command.UserId, command.Rq.Title, command.Rq.Content, command.Rq.ContentSummary, command.Rq.IsPreviewable);
    }
}
