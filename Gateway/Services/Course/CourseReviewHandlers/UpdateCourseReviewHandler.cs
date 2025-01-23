using Contract.Domain.CourseAggregate;
using Contract.Helpers;
using Contract.Requests.Library.CourseReviewRequests;
using Contract.Requests.Shared.BaseDtos.Reviews;
using Core.Helpers;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Course.CourseReviewHandlers;

public class UpdateCourseReviewHandler : RequestHandler<UpdateCourseReviewCommand, HealpathyContext>
{
    public UpdateCourseReviewHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }

    public override async Task<Result> Handle(UpdateCourseReviewCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.CourseReviews.FindExt(command.Rq.Id);
        if (entity is null)
            return NotFound(string.Empty);

        ApplyChanges(entity, command.Rq);
        try
        {
            if (command.AddedMedias is not null && command.AddedMedias.Count > 0)
                _context.Multimedia.AddRange(command.AddedMedias);
            if (command.RemovedMedias is not null && command.RemovedMedias.Count > 0)
                await _context.Multimedia.DeleteExt(command.RemovedMedias);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private void ApplyChanges(CourseReview entity, UpdateReviewDto dto)
    {
        if (dto.Rating is not null)
            entity.Rating = (byte)dto.Rating;
        if (dto.Content is not null)
            entity.Content = dto.Content;
        entity.LastModificationTime = TimeHelper.Now;
    }
}
