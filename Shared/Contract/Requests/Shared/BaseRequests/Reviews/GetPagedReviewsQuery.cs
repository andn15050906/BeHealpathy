using Contract.Requests.Shared.BaseDtos.Reviews;
using Contract.Responses.Shared;

namespace Contract.Requests.Shared.BaseRequests.Reviews;

public abstract class GetPagedReviewsQuery : IRequest<Result<PagedResult<ReviewModel>>>
{
    public QueryReviewDto Rq { get; init; }



    public GetPagedReviewsQuery(QueryReviewDto rq)
    {
        Rq = rq;
    }
}