using Contract.Domain.CourseAggregate;
using Contract.Helpers;
using Contract.Requests.Courses.LectureRequests;
using Core.Helpers;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Course.LectureHandlers;

public class UpdateLectureHandler : RequestHandler<UpdateLectureCommand, HealpathyContext>
{
    public UpdateLectureHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }

    public override async Task<Result> Handle(UpdateLectureCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.Lectures.FindExt(command.Rq.Id);
        if (entity is null)
            return NotFound(string.Empty);

        ApplyChanges(entity, command);
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

    private void ApplyChanges(Lecture entity, UpdateLectureCommand command)
    {
        entity.Title = command.Rq.Title ?? string.Empty;
        entity.Content = command.Rq.Content ?? string.Empty;
        entity.ContentSummary = command.Rq.ContentSummary ?? string.Empty;
        entity.LastModificationTime = TimeHelper.Now;

        entity.Index = command.Rq.Index ?? 0;
        entity.LectureType = command.Rq.LectureType ?? string.Empty;
        entity.MetaData = command.Rq.MetaData ?? string.Empty;
    }
}
