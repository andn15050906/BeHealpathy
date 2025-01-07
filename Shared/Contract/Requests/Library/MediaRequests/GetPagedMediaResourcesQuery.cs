using Contract.Requests.Library.MediaRequests.Dtos;
using Contract.Responses.Library;

namespace Contract.Requests.Library.MediaRequests;

public sealed class GetPagedMediaResourcesQuery : IRequest<Result<PagedResult<MediaResourceModel>>>
{
    public QueryMediaResourceDto Rq { get; init; }
    public Guid UserId { get; init; }



    public GetPagedMediaResourcesQuery(QueryMediaResourceDto rq, Guid userId)
    {
        Rq = rq;
        UserId = userId;
    }
}
