using Contract.Domain.CommunityAggregate;
using Contract.Helpers;
using Contract.Requests.Community.ChatMessageRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Community.MessageHandlers;

public sealed class CreateChatMessageHandler : RequestHandler<CreateChatMessageCommand, HealpathyContext>
{
    public CreateChatMessageHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result> Handle(CreateChatMessageCommand command, CancellationToken cancellationToken)
    {
        ChatMessage entity = Adapt(command);

        try
        {
            var commentTask = _context.ChatMessages.InsertExt(entity);
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

    private ChatMessage Adapt(CreateChatMessageCommand command)
    {
        return new ChatMessage(command.Id, command.UserId, command.Rq.Content, command.Rq.ConversationId);
    }
}