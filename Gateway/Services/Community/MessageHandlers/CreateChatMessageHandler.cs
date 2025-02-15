using Contract.BusinessRules;
using Contract.Domain.CommunityAggregate;
using Contract.Helpers;
using Contract.Requests.Community.ChatMessageRequests;
using Contract.Requests.Community.ConversationRequests;
using Contract.Requests.Community.ConversationRequests.Dtos;
using Contract.Responses.Community;
using Contract.Responses.Shared;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Community.MessageHandlers;

public sealed class CreateChatMessageHandler : RequestHandler<CreateChatMessageCommand, ChatMessageModel, HealpathyContext>
{
    public CreateChatMessageHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<ChatMessageModel>> Handle(CreateChatMessageCommand command, CancellationToken cancellationToken)
    {
        // AI userId ...

        // Special case where Conversation is not created, see CreateConversationHandler
        if (command.Rq.ConversationId == command.UserId)
        {
            var conversation = await _context.Conversations.FindExt(command.Rq.ConversationId);
            if (conversation is null)
            {
                CreateConversationCommand convCommand = new(
                    command.Rq.ConversationId,
                    new CreateConversationDto()
                    {
                        IsPrivate = true,
                        Members = [
                            new CreateConversationMemberDto { IsAdmin = true, UserId = command.UserId },
                            new CreateConversationMemberDto { IsAdmin = true, UserId = PreSet.SystemUserId },
                        ],
                        Title = "Your AI partner"
                    },
                    command.UserId,
                    null
                );
                conversation = Adapt(convCommand);
                await _context.Conversations.InsertExt(conversation);
            }
        }

        ChatMessage entity = Adapt(command);

        try
        {
            var commentTask = _context.ChatMessages.InsertExt(entity);
            var mediaTask = command.Medias is not null && command.Medias.Count > 0
                ? _context.Multimedia.AddRangeAsync(command.Medias.Where(_ => _ is not null))
                : Task.CompletedTask;
            await Task.WhenAll(commentTask, mediaTask);
            await _context.SaveChangesAsync();

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

    private static ChatMessage Adapt(CreateChatMessageCommand command)
    {
        return new ChatMessage(command.Id, command.UserId, command.Rq.Content ?? string.Empty, command.Rq.ConversationId);
    }

    private static Conversation Adapt(CreateConversationCommand command)
    {
        var members = command.Rq.Members.Select(_ => new ConversationMember(_.UserId, command.Id, _.IsAdmin)).ToList();
        return new Conversation(command.Id, command.UserId, command.Rq.Title, command.Rq.IsPrivate, command.Media?.Url ?? string.Empty, members);
    }
}