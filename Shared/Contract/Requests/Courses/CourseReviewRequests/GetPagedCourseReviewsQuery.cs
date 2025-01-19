using Contract.Requests.Shared.BaseDtos.Reviews;
using Contract.Requests.Shared.BaseRequests.Reviews;

namespace Contract.Requests.Library.CourseReviewRequests;

public sealed class GetPagedCourseReviewsQuery : GetPagedReviewsQuery
{
    public GetPagedCourseReviewsQuery(QueryReviewDto rq) : base(rq)
    {
    }
}