using Contract.Requests.Courses.EnrollmentRequests.Dtos;
using Contract.Responses.Courses;

namespace Contract.Requests.Courses.EnrollmentRequests;

public sealed class GetPagedEnrollmentsQuery : IRequest<Result<PagedResult<EnrollmentModel>>>
{
    public QueryEnrollmentDto Rq { get; init; }

    public Guid UserId { get; init; }



    public GetPagedEnrollmentsQuery(QueryEnrollmentDto rq, Guid userId)
    {
        Rq = rq;
        UserId = userId;
    }
}