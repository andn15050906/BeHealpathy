using Contract.Domain.CommunityAggregate;
using Contract.Helpers;
using Contract.Requests.Community.ConversationRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Community.ConversationHandlers;

public sealed class CreateConversationHandler : RequestHandler<CreateConversationCommand, HealpathyContext>
{
    public CreateConversationHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    // See also: CreateChatMessageHandler
    public override async Task<Result> Handle(CreateConversationCommand command, CancellationToken cancellationToken)
    {
        Conversation entity = Adapt(command);

        try
        {
            var conversationTask = _context.Conversations.InsertExt(entity);
            var mediaTask = Task.CompletedTask;
            if (command.Media is not null)
                mediaTask = _context.Multimedia.InsertExt(command.Media);
            await Task.WhenAll(conversationTask, mediaTask);
            await _context.SaveChangesAsync(cancellationToken);
            return Created();
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private static Conversation Adapt(CreateConversationCommand command)
    {
        var id = Guid.NewGuid();
        var members = command.Rq.Members.Select(_ => new ConversationMember(_.UserId, id, _.IsAdmin)).ToList();
        return new Conversation(command.Id, command.UserId, command.Rq.Title, command.Rq.IsPrivate, command.Media?.Url ?? string.Empty, members);
    }
}