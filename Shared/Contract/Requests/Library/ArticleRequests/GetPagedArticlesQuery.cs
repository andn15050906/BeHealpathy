using Contract.Requests.Library.ArticleRequests.Dtos;
using Contract.Responses.Library;

namespace Contract.Requests.Community.ArticleRequests;

public sealed class GetPagedArticlesQuery : IRequest<Result<PagedResult<ArticleModel>>>
{
    public QueryArticleDto Rq { get; init; }



    public GetPagedArticlesQuery(QueryArticleDto rq)
    {
        Rq = rq;
    }
}