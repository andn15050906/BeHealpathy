using Contract.Domain.Shared.CommentBase;
using Contract.Helpers;
using Contract.Requests.Library.ArticleCommentRequests;
using Contract.Requests.Shared.BaseDtos.Comments;
using Contract.Responses.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Gateway.Services.Library.ArticleCommentHandlers;

public sealed class GetPagedArticleCommentsHandler : RequestHandler<GetPagedArticleCommentsQuery, PagedResult<CommentModel>, HealpathyContext>
{
    public GetPagedArticleCommentsHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override async Task<Result<PagedResult<CommentModel>>> Handle(GetPagedArticleCommentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                CommentModel.MapExpression,
                GetPredicate(request.Rq),
                request.Rq.PageIndex,
                request.Rq.PageSize,
                false,
                _ => _.Creator
            );
            var result = await query.ExecuteWithOrderBy(_ => _.LastModificationTime);

            List<Guid> sourceIds = result.Items.Select(_ => _.Id).ToList();
            var medias = await _context.Multimedia
                .Where(_ => sourceIds.Contains(_.SourceId) && !_.IsDeleted)
                .Select(MultimediaModel.MapExpression)
                .ToListAsync();
            foreach (var entity in result.Items)
                entity.Medias = medias.Where(_ => _.SourceId == entity.Id).ToList() ?? [];

            var reactions = await _context.ArticleReactions
                .Where(_ => sourceIds.Contains(_.SourceId) || !_.IsDeleted)
                .Select(ReactionModel.MapExpression)
                .ToListAsync();
            foreach (var entity in result.Items)
                entity.Reactions = reactions.Where(_ => _.SourceId == entity.Id).ToList() ?? [];

            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<Comment, bool>>? GetPredicate(QueryCommentDto dto)
    {
        if (dto.SourceId is not null)
            return _ => _.SourceId == dto.SourceId && !_.IsDeleted;
        if (dto.CreatorId is not null)
            return _ => _.CreatorId == dto.CreatorId && !_.IsDeleted;

        return _ => !_.IsDeleted;
    }
}
