namespace Contract.Requests.Progress.SubmissionRequests.Dtos;

public sealed class QuerySubmissionDto : PagingQueryDto
{
    public Guid? Id { get; set; }

    public Guid? CreatorId { get; set; }

    public Guid? SurveyId { get; set; }
}
