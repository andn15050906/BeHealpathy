using Contract.Domain.CourseAggregate;
using Contract.Helpers;
using Contract.Requests.Courses.LectureCommentRequests;
using Contract.Responses.Shared;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Course.LectureCommentHandlers;

public sealed class CreateLectureCommentHandler : RequestHandler<CreateLectureCommentCommand, CommentModel, HealpathyContext>
{
    public CreateLectureCommentHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<CommentModel>> Handle(CreateLectureCommentCommand command, CancellationToken cancellationToken)
    {
        LectureComment entity = Adapt(command);

        try
        {
            var commentTask = _context.LectureComments.InsertExt(entity);
            var mediaTask = Task.CompletedTask;
            if (command.Medias is not null)
            {
                var medias = command.Medias.Where(_ => _ is not null);
                if (medias.Any())
                    mediaTask = _context.Multimedia.AddRangeAsync(medias);
            }
            await Task.WhenAll(commentTask, mediaTask);
            await _context.SaveChangesAsync(cancellationToken);

            var model = CommentModel.MapFunc(entity);
            if (command.Medias is not null)
            {
                model.Medias = command.Medias.Select(_ => MultimediaModel.MapFunc(_));
            }
            return Created(model);
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private static LectureComment Adapt(CreateLectureCommentCommand command)
    {
        return new LectureComment(command.Id, command.UserId, command.Rq.SourceId, command.Rq.Content);
    }
}