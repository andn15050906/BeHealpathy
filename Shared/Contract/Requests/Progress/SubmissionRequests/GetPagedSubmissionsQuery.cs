using Contract.Requests.Progress.SubmissionRequests.Dtos;
using Contract.Responses.Progress;

namespace Contract.Requests.Progress.SubmissionRequests;

public sealed class GetPagedSubmissionsQuery : IRequest<Result<PagedResult<SubmissionModel>>>
{
    public QuerySubmissionDto Rq { get; init; }
    public Guid UserId { get; init; }



    public GetPagedSubmissionsQuery(QuerySubmissionDto rq, Guid userId)
    {
        Rq = rq;
        UserId = userId;
    }
}
