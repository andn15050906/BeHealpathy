using Contract.Helpers;
using Contract.Requests.Library.ArticleReactionRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.ArticleReactionHandlers;

public sealed class DeleteArticleReactionHandler : RequestHandler<DeleteArticleReactionCommand, HealpathyContext>
{
    public DeleteArticleReactionHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result> Handle(DeleteArticleReactionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ArticleReactions.FindExt(request.Id);
        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != request.UserId)
            return Unauthorized(string.Empty);

        try
        {
            _context.ArticleReactions.SoftDeleteExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}