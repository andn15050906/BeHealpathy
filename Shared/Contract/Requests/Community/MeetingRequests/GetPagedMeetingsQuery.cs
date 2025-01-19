using Contract.Requests.Community.MeetingRequests.Dtos;
using Contract.Responses.Community;

namespace Contract.Requests.Community.MeetingRequests;

public sealed class GetPagedMeetingsQuery : IRequest<Result<PagedResult<MeetingModel>>>
{
    public QueryMeetingDto Rq { get; init; }
    public Guid UserId { get; init; }



    public GetPagedMeetingsQuery(QueryMeetingDto rq, Guid userId)
    {
        Rq = rq;
        UserId = userId;
    }
}