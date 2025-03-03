namespace Contract.Requests.Shared.BaseDtos.Comments;

public sealed class QueryCommentDto : PagingQueryDto
{
    public Guid? SourceId { get; set; }
    public Guid? CreatorId { get; set; }

    public TargetFeedbackEntity TargetEntity { get; set; }
}
