using Contract.Requests.Shared.Base;

namespace Contract.Requests.Shared.Comments;

public sealed class QueryCommentDto : PagingQueryDto
{
    public Guid? SourceId { get; set; }
    public Guid? CreatorId { get; set; }
}
