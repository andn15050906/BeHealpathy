using Contract.Helpers;
using Contract.Requests.Community.MessageReactionRequests;
using Contract.Responses.Shared;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Community.MessageReactionHandlers;

public sealed class DeleteMessageReactionHandler : RequestHandler<DeleteMessageReactionCommand, ReactionModel, HealpathyContext>
{
    public DeleteMessageReactionHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result<ReactionModel>> Handle(DeleteMessageReactionCommand request, CancellationToken cancellationToken)
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
            return Ok(ReactionModel.MapFunc(entity));
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}