using Contract.Helpers;
using Contract.Requests.Progress.DiaryNoteRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Progress.DiaryNoteHandlers;

public sealed class DeleteDiaryNoteHandler : RequestHandler<DeleteDiaryNoteCommand, HealpathyContext>
{
    public DeleteDiaryNoteHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result> Handle(DeleteDiaryNoteCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.DiaryNotes.FindExt(request.Id);
        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != request.UserId)
            return Unauthorized(string.Empty);

        try
        {
            _context.DiaryNotes.SoftDeleteExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}