using Contract.Domain.LibraryAggregate;
using Contract.Helpers;
using Contract.Requests.Library.ArticleReactionRequests;
using Contract.Responses.Shared;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.ArticleReactionHandlers;

public sealed class CreateArticleReactionHandler(HealpathyContext context, IAppLogger logger, IEventCache cache)
    : RequestHandler<CreateArticleReactionCommand, ReactionModel, HealpathyContext>(context, logger, cache)
{
    public override async Task<Result<ReactionModel>> Handle(CreateArticleReactionCommand command, CancellationToken cancellationToken)
    {
        ArticleReaction entity = Adapt(command);

        try
        {
            await _context.ArticleReactions.InsertExt(entity);
            await _context.SaveChangesAsync(cancellationToken);

            _cache.Add(command.UserId, new Events.ArticleReaction_Created(entity.Id));
            return Created(ReactionModel.MapFunc(entity));
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return ServerError(ex.Message);
        }
    }

    private static ArticleReaction Adapt(CreateArticleReactionCommand command)
    {
        return new ArticleReaction(command.Id, command.UserId, command.Rq.SourceId, command.Rq.Content);
    }
}
