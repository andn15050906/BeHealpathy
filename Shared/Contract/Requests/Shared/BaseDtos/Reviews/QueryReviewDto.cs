namespace Contract.Requests.Shared.BaseDtos.Reviews;

public sealed class QueryReviewDto : PagingQueryDto
{
    public Guid? SourceId { get; set; }
    public Guid? CreatorId { get; set; }

    public TargetFeedbackEntity TargetEntity { get; set; }
}
