using Contract.Requests.Shared.BaseDtos.Comments;
using Contract.Responses.Shared;

namespace Contract.Requests.Shared.BaseRequests.Comments;

public class GetPagedCommentsQuery : IRequest<Result<PagedResult<CommentModel>>>
{
    public QueryCommentDto Rq { get; init; }



    public GetPagedCommentsQuery(QueryCommentDto rq)
    {
        Rq = rq;
    }
}
