using Contract.Helpers;
using Contract.Requests.Community.MessageReactionRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Community.MessageReactionHandlers;

public sealed class DeleteMessageReactionHandler : RequestHandler<DeleteMessageReactionCommand, HealpathyContext>
{
    public DeleteMessageReactionHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result> Handle(DeleteMessageReactionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.MessageReactions.FindExt(request.Id);
        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != request.UserId)
            return Unauthorized(string.Empty);

        try
        {
            _context.MessageReactions.SoftDeleteExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}