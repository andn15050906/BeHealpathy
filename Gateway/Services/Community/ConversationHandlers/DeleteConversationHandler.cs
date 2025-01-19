using Contract.Helpers;
using Contract.Requests.Community.ConversationRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Community.ConversationHandlers;

public sealed class DeleteConversationHandler : RequestHandler<DeleteConversationCommand, HealpathyContext>
{
    public DeleteConversationHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result> Handle(DeleteConversationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Conversations.FindExt(request.Id);
        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != request.UserId)
            return Unauthorized(string.Empty);

        try
        {
            _context.Conversations.SoftDeleteExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}