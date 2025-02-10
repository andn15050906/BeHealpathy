using Contract.Domain.ProgressAggregates;
using Contract.Helpers;
using Contract.Requests.Progress.DiaryNoteRequests;
using Contract.Responses.Progress;
using Contract.Responses.Shared;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.DiaryNoteHandlers;

public sealed class CreateDiaryNoteHandler : RequestHandler<CreateDiaryNoteCommand, DiaryNoteModel, HealpathyContext>
{
    public CreateDiaryNoteHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



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