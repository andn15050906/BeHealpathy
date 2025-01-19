namespace Contract.Requests.Progress.McqRequests.Dtos;

public sealed class QueryMcqQuestionDto : PagingQueryDto
{
    public Guid SurveyId { get; set; }
}
