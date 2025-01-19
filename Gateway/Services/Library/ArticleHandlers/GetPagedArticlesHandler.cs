using Contract.Domain.LibraryAggregate;
using Contract.Helpers;
using Contract.Requests.Community.ArticleRequests;
using Contract.Requests.Library.ArticleRequests.Dtos;
using Contract.Responses.Library;
using Contract.Responses.Shared;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Gateway.Services.Library.ArticleHandlers;

public sealed class GetPagedArticlesHandler : RequestHandler<GetPagedArticlesQuery, PagedResult<ArticleModel>, HealpathyContext>
{
    public GetPagedArticlesHandler(HealpathyContext context, IAppLogger logger) : base(context, logger) { }



    public override async Task<Result<PagedResult<ArticleModel>>> Handle(GetPagedArticlesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _context.GetPagingQuery(
                ArticleModel.MapExpression,
                GetPredicate(request.Rq),
                request.Rq.PageIndex,
                request.Rq.PageSize,
                false,
                _ => _.Tags, _ => _.Comments, _ => _.Reactions, _ => _.Creator
            );
            var result = await query.ExecuteWithOrderBy(_ => _.LastModificationTime);

            List<Guid> thumbSourceIds = result.Items.Select(_ => _.Id).ToList();
            List<Guid> sectionSourceIds = result.Items.SelectMany(_ => _.Sections).Select(_ => _.Id).ToList();
            thumbSourceIds.AddRange(sectionSourceIds);
            var sourceIds = thumbSourceIds;
            var medias = await _context.Multimedia
                .Where(_ => sourceIds.Contains(_.SourceId) && !_.IsDeleted)
                .Select(MultimediaModel.MapExpression)
                .ToListAsync(cancellationToken);

            var reactions = await _context.ArticleReactions
                .Where(_ => sourceIds.Contains(_.SourceId) || !_.IsDeleted)
                .Select(ReactionModel.MapExpression)
                .ToListAsync(cancellationToken);

            foreach (var article in result.Items)
            {
                article.Thumb = medias.FirstOrDefault(_ => _.SourceId == article.Id);
                foreach (var section in article.Sections)
                    section.Media = medias.FirstOrDefault(_ => _.SourceId == section.Id);
                article.Reactions = reactions.Where(_ => _.SourceId == article.Id).ToList() ?? [];
            }

            return ToQueryResult(result);
        }
        catch (Exception ex)
        {
            return ServerError(ex.Message);
        }
    }

    private Expression<Func<Article, bool>>? GetPredicate(QueryArticleDto dto)
    {
        if (dto.Title is not null)
            return _ => _.Title.Contains(dto.Title, StringComparison.OrdinalIgnoreCase) && !_.IsDeleted;
        if (dto.Status is not null)
            return _ => _.Status == dto.Status && !_.IsDeleted;

        if (dto.Tags is not null)
            return _ => _.Tags.Any(tag => dto.Tags.Contains(tag.Id)) && !_.IsDeleted;

        return _ => !_.IsDeleted;
    }
}