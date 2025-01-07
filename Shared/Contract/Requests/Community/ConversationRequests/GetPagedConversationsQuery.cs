using Contract.Messaging.Models;
using Contract.Requests.Community.ConversationRequests.Dtos;
using Contract.Responses.Community;

namespace Contract.Requests.Community.ConversationRequests;

public sealed class GetPagedConversationsQuery : IRequest<Result<PagedResult<ConversationModel>>>
{
    public QueryConversationDto Rq { get; init; }
    public Guid UserId { get; init; }



    public GetPagedConversationsQuery(QueryConversationDto rq, Guid userId)
    {
        Rq = rq;
        UserId = userId;
    }
}
