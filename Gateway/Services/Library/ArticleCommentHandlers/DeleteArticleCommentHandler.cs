using Contract.Helpers;
using Contract.Requests.Library.ArticleCommentRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.ArticleCommentHandlers;

public sealed class DeleteArticleCommentHandler : RequestHandler<DeleteArticleCommentCommand, HealpathyContext>
{
    public DeleteArticleCommentHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result> Handle(DeleteArticleCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.ArticleComments.FindExt(request.Id);
        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != request.UserId)
            return Unauthorized(string.Empty);

        try
        {
            _context.ArticleComments.SoftDeleteExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}