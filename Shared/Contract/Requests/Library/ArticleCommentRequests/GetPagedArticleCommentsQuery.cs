using Contract.Requests.Shared.BaseDtos.Comments;
using Contract.Requests.Shared.BaseRequests.Comments;

namespace Contract.Requests.Library.ArticleCommentRequests;

public sealed class GetPagedArticleCommentsQuery : GetPagedCommentsQuery
{
    public GetPagedArticleCommentsQuery(QueryCommentDto rq) : base(rq)
    {
    }
}
