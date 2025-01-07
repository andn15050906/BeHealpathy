using Contract.Requests.Community.ChatMessageRequests.Dtos;
using Contract.Responses.Community;

namespace Contract.Requests.Community.ChatMessageRequests;

public sealed class GetPagedChatMessagesQuery : IRequest<Result<PagedResult<ChatMessageModel>>>
{
    public QueryChatMessageDto Rq { get; init; }
    public Guid UserId { get; init; }



    public GetPagedChatMessagesQuery(QueryChatMessageDto rq, Guid userId)
    {
        Rq = rq;
        UserId = userId;
    }
}
