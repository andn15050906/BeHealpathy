using Contract.Domain.ProgressAggregates;
using Contract.Helpers;
using Contract.Requests.Progress.DiaryNoteRequests;
using Contract.Responses.Progress;
using Contract.Responses.Shared;
using Infrastructure.DataAccess.SQLServer.Helpers;
using System.Text.Json;

namespace Gateway.Services.Progress.DiaryNoteHandlers;

public sealed class CreateDiaryNoteHandler(HealpathyContext context, IAppLogger logger, IEventCache cache)
    : RequestHandler<CreateDiaryNoteCommand, DiaryNoteModel, HealpathyContext>(context, logger, cache)
{
    public override async Task<Result<DiaryNoteModel>> Handle(CreateDiaryNoteCommand command, CancellationToken cancellationToken)
    {
        DiaryNote entity = Adapt(command);

        try
        {
            var commentTask = _context.DiaryNotes.InsertExt(entity);
            var mediaTask = command.Medias is not null
                ? _context.Multimedia.AddRangeAsync(command.Medias.Where(_ => _ is not null))
                : Task.CompletedTask;
            await Task.WhenAll(commentTask, mediaTask);
            await _context.SaveChangesAsync(cancellationToken);

            var model = DiaryNoteModel.MapFunc(entity);
            if (command.Medias is not null)
            {
                model.Medias = command.Medias.Select(_ => MultimediaModel.MapFunc(_));
            }

            if (command.Rq.Mood is not null)
                _cache.Add(command.UserId, new Events.General_Activity_Created(JsonSerializer.Serialize(new { action = "Mood_Updated", content = command.Rq.Mood })));
            else
                _cache.Add(command.UserId, new Events.DiaryNote_Created(entity.Id));
            return Created(model);
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private static DiaryNote Adapt(CreateDiaryNoteCommand command)
    {
        return new DiaryNote(
            command.Id, command.UserId,
            command.Rq.Title ?? string.Empty, command.Rq.Content ?? string.Empty,
            command.Rq.Mood ?? string.Empty, command.Rq.Theme ?? string.Empty
        );
    }
}