using Contract.Helpers;
using Contract.Requests.Community.ArticleRequests;
using Infrastructure.DataAccess.SQLServer.Helpers;

namespace Gateway.Services.Library.ArticleHandlers;

public class DeleteArticleHandler : RequestHandler<DeleteArticleCommand, HealpathyContext>
{
    public DeleteArticleHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Articles.FindExt(request.Id);
        if (entity is null)
            return NotFound(string.Empty);
        if (entity.CreatorId != request.UserId)
            return Unauthorized(string.Empty);

        try
        {
            _context.Articles.SoftDeleteExt(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return Ok();
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }
}
