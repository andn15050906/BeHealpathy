using Contract.Requests.Shared.Base;

namespace Contract.Requests.Community.ConversationRequests.Dtos;

public sealed class QueryConversationDto : PagingQueryDto
{
    public IEnumerable<Guid>? ConversationIds { get; set; }
    public string? Title { get; set; }
}
