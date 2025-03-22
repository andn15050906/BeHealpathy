using Contract.Domain.CommunityAggregate;
using Contract.Helpers;
using Contract.Requests.Community.MessageReactionRequests;
using Contract.Responses.Shared;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Community.MessageReactionHandlers;

public sealed class CreateMessageReactionHandler(HealpathyContext context, IAppLogger logger, IEventCache cache)
    : RequestHandler<CreateMessageReactionCommand, ReactionModel, HealpathyContext>(context, logger, cache)
{
    public override async Task<Result<ReactionModel>> Handle(CreateMessageReactionCommand command, CancellationToken cancellationToken)
    {
        MessageReaction entity = Adapt(command);

        try
        {
            await _context.MessageReactions.InsertExt(entity);
            await _context.SaveChangesAsync(cancellationToken);

            _cache.Add(command.UserId, new Events.MessageReaction_Created(entity.Id));
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