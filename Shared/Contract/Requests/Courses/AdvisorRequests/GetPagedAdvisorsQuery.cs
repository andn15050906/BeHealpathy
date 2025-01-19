using Contract.Requests.Courses.AdvisorRequests.Dtos;
using Contract.Responses.Courses;

namespace Contract.Requests.Courses.AdvisorRequests;

public sealed class GetPagedAdvisorsQuery : IRequest<Result<PagedResult<AdvisorModel>>>
{
    public QueryAdvisorDto Rq { get; init; }



    public GetPagedAdvisorsQuery(QueryAdvisorDto rq)
    {
        Rq = rq;
    }
}