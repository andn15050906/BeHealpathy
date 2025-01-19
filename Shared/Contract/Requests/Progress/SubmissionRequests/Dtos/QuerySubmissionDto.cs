namespace Contract.Requests.Progress.SubmissionRequests.Dtos;

public sealed class QuerySubmissionDto : PagingQueryDto
{
    public Guid? CreatorId { get; set; }

    public Guid? SurveyId { get; set; }
}
