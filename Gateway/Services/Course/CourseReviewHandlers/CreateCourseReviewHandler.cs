using Contract.Domain.CourseAggregate;
using Contract.Helpers;
using Contract.Requests.Library.CourseReviewRequests;
using Contract.Responses.Shared;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Course.CourseReviewHandlers;

public class CreateCourseReviewHandler : RequestHandler<CreateCourseReviewCommand, ReviewModel, HealpathyContext>
{
    public CreateCourseReviewHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }

    public override async Task<Result<ReviewModel>> Handle(CreateCourseReviewCommand command, CancellationToken cancellationToken)
    {
        var entity = Adapt(command);
        try
        {
            var reviewTask = _context.CourseReviews.InsertExt(entity);
            var mediaTask = command.Medias is not null
                ? _context.Multimedia.AddRangeAsync(command.Medias.Where(_ => _ is not null))
                : Task.CompletedTask;
            await Task.WhenAll(reviewTask, mediaTask);
            await _context.SaveChangesAsync(cancellationToken);

            var model = ReviewModel.MapFunc(entity);
            if (command.Medias is not null)
            {
                model.Medias = command.Medias.Select(_ => MultimediaModel.MapFunc(_));
            }
            return Created(model);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private static CourseReview Adapt(CreateCourseReviewCommand command)
    {
        return new CourseReview(command.Id, command.UserId, command.Rq.SourceId, command.Rq.Content, command.Rq.Rating);
    }
}
