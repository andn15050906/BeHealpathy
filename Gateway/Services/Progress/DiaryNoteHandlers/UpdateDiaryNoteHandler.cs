using Contract.Domain.ToolAggregate;
using Contract.Helpers;
using Contract.Requests.Progress.DiaryNoteRequests;
using Core.Helpers;
using Infrastructure.DataAccess.SQLServer.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Gateway.Services.Progress.DiaryNoteHandlers;

public sealed class UpdateDiaryNoteHandler(HealpathyContext context, IAppLogger logger, IEventCache cache)
    : RequestHandler<UpdateDiaryNoteCommand, HealpathyContext>(context, logger, cache)
{
    public override async Task<Result> Handle(UpdateDiaryNoteCommand command, CancellationToken cancellationToken)
    {
        var entity = await _context.DiaryNotes.FirstOrDefaultAsync(_ => _.Id == command.Rq.Id);

        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != command.UserId)
            return Unauthorized(string.Empty);

        try
        {
            entity = ApplyChanges(entity, command);

            if (command.AddedMedias is not null && command.AddedMedias.Count > 0)
                _context.Multimedia.AddRange(command.AddedMedias);
            if (command.RemovedMedias is not null && command.RemovedMedias.Count > 0)
                await _context.Multimedia.DeleteExt(command.RemovedMedias);

            await _context.SaveChangesAsync(cancellationToken);

            if (command.Rq.Mood is not null)
                _cache.Add(command.UserId, new Events.General_Activity_Created(JsonSerializer.Serialize(new { action = "Mood_Updated", content = command.Rq.Mood })));
            else
                _cache.Add(command.UserId, new Events.DiaryNote_Updated(entity.Id));
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private DiaryNote ApplyChanges(DiaryNote entity, UpdateDiaryNoteCommand command)
    {
        if (command.Rq.Title is not null)
            entity.Title = command.Rq.Title;
        if (command.Rq.Content is not null)
            entity.Content = command.Rq.Content;
        if (command.Rq.Mood is not null)
            entity.Mood = command.Rq.Mood;
        if (command.Rq.Theme is not null)
            entity.Theme = command.Rq.Theme;
        entity.LastModifierId = command.UserId;
        entity.LastModificationTime = TimeHelper.Now;

        return entity;
    }
}