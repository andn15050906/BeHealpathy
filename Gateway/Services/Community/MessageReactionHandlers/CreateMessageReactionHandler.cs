using Contract.Domain.CommunityAggregate;
using Contract.Helpers;
using Contract.Requests.Community.MessageReactionRequests;
using Contract.Responses.Shared;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Community.MessageReactionHandlers;

public sealed class CreateMessageReactionHandler : RequestHandler<CreateMessageReactionCommand, ReactionModel, HealpathyContext>
{
    public CreateMessageReactionHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<ReactionModel>> Handle(CreateMessageReactionCommand command, CancellationToken cancellationToken)
    {
        MessageReaction entity = Adapt(command);

        try
        {
            await _context.MessageReactions.InsertExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Created(ReactionModel.MapFunc(entity));
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private static MessageReaction Adapt(CreateMessageReactionCommand command)
    {
        return new MessageReaction(command.Id, command.UserId, command.Rq.SourceId, command.Rq.Content);
    }
}