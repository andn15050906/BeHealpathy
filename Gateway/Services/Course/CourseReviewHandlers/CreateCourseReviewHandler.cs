using Contract.Domain.CourseAggregate;
using Contract.Helpers;
using Contract.Requests.Library.CourseReviewRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Gateway.Services.Course.CourseReviewHandlers;

public class CreateCourseReviewHandler : RequestHandler<CreateCourseReviewCommand, HealpathyContext>
{
    public CreateCourseReviewHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }

    public override async Task<Result> Handle(CreateCourseReviewCommand request, CancellationToken cancellationToken)
    {
        var entity = Adapt(request);
        try
        {
            var reviewTask = _context.CourseReviews.InsertExt(entity);
            var mediaTask = request.Medias is not null
                ? _context.Multimedia.AddRangeAsync(request.Medias.Where(_ => _ is not null))
                : Task.CompletedTask;
            await Task.WhenAll(reviewTask, mediaTask);
            await _context.SaveChangesAsync(cancellationToken);
            return Created();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private CourseReview Adapt(CreateCourseReviewCommand command)
    {
        return new CourseReview(command.Id, command.UserId, command.Rq.SourceId, command.Rq.Content, command.Rq.Rating);
    }
}
