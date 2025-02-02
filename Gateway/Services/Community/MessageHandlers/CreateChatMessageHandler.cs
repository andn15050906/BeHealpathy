using Contract.Domain.CommunityAggregate;
using Contract.Helpers;
using Contract.Requests.Community.ChatMessageRequests;
using Contract.Responses.Community;
using Contract.Responses.Shared;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Community.MessageHandlers;

public sealed class CreateChatMessageHandler : RequestHandler<CreateChatMessageCommand, ChatMessageModel, HealpathyContext>
{
    public CreateChatMessageHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<ChatMessageModel>> Handle(CreateChatMessageCommand command, CancellationToken cancellationToken)
    {
        ChatMessage entity = Adapt(command);

        try
        {
            var commentTask = _context.ChatMessages.InsertExt(entity);
            var mediaTask = command.Medias is not null && command.Medias.Count > 0
                ? _context.Multimedia.AddRangeAsync(command.Medias.Where(_ => _ is not null))
                : Task.CompletedTask;
            await Task.WhenAll(commentTask, mediaTask);
            await _context.SaveChangesAsync(cancellationToken);

            var model = ChatMessageModel.MapFunc(entity);
            if (command.Medias is not null)
            {
                model.Attachments = command.Medias.Select(_ => MultimediaModel.MapFunc(_));
            }
            return Created(model);
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private ChatMessage Adapt(CreateChatMessageCommand command)
    {
        return new ChatMessage(command.Id, command.UserId, command.Rq.Content ?? string.Empty, command.Rq.ConversationId);
    }
}