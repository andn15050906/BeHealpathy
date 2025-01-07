namespace Contract.Requests.Community.ChatMessageRequests.Dtos;

public sealed class QueryChatMessageDto : PagingQueryDto
{
    public Guid ConversationId { get; set; }
}
