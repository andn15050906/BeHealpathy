namespace Contract.Requests.Community.ConversationRequests.Dtos;

public sealed class QueryConversationDto : PagingQueryDto
{
    public Guid? CreatorId { get; set; }

    public string? Title { get; set; }
    public List<Guid>? Members { get; set; }

    // Exceptional
    public List<Guid>? ConversationIds { get; set; }
}
