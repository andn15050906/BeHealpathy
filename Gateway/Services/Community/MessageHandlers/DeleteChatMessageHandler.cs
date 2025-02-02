using Contract.Helpers;
using Contract.Requests.Community.ChatMessageRequests;
using Contract.Responses.Community;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Community.MessageHandlers;

public sealed class DeleteChatMessageHandler : RequestHandler<DeleteChatMessageCommand, ChatMessageModel, HealpathyContext>
{
    public DeleteChatMessageHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result<ChatMessageModel>> Handle(DeleteChatMessageCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ChatMessages.FindExt(request.Id);
        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != request.UserId)
            return Unauthorized(string.Empty);

        try
        {
            _context.ChatMessages.SoftDeleteExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok(ChatMessageModel.MapFunc(entity));
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}