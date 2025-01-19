using Contract.Domain.ProgressAggregates;
using Contract.Helpers;
using Contract.Requests.Progress.DiaryNoteRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.DiaryNoteHandlers;

public sealed class CreateDiaryNoteHandler : RequestHandler<CreateDiaryNoteCommand, HealpathyContext>
{
    public CreateDiaryNoteHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result> Handle(CreateDiaryNoteCommand command, CancellationToken cancellationToken)
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
            return Created();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private DiaryNote Adapt(CreateDiaryNoteCommand command)
    {
        return new DiaryNote(
            command.Id, command.UserId,
            command.Rq.Title, command.Rq.Content, command.Rq.Mood, command.Rq.Theme
        );
    }
}