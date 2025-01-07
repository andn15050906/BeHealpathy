using Contract.Requests.Shared.Base;

namespace Contract.Requests.Shared.Reviews;

public sealed class QueryReviewDto : PagingQueryDto
{
    public Guid? SourceId { get; set; }
    public Guid? CreatorId { get; set; }
}
