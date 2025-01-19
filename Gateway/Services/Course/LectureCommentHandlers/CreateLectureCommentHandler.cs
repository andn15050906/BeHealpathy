using Contract.Domain.CourseAggregate;
using Contract.Helpers;
using Contract.Requests.Courses.LectureCommentRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Course.LectureCommentHandlers;

public sealed class CreateLectureCommentHandler : RequestHandler<CreateLectureCommentCommand, HealpathyContext>
{
    public CreateLectureCommentHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result> Handle(CreateLectureCommentCommand command, CancellationToken cancellationToken)
    {
        LectureComment entity = Adapt(command);

        try
        {
            var commentTask = _context.LectureComments.InsertExt(entity);
            var mediaTask = command.Medias is not null
                ? _context.Multimedia.AddRangeAsync(command.Medias.Where(_ => _ is not null))
                : Task.CompletedTask;
            await Task.WhenAll(commentTask, mediaTask);
            await _context.SaveChangesAsync(cancellationToken);
            return Created();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private LectureComment Adapt(CreateLectureCommentCommand command)
    {
        return new LectureComment(command.Id, command.UserId, command.Rq.SourceId, command.Rq.Content);
    }
}