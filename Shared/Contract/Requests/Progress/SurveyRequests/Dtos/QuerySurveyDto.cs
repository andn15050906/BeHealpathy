namespace Contract.Requests.Progress.SurveyRequests.Dtos;

public sealed class QuerySurveyDto : PagingQueryDto
{
    public string? Name { get; set; }
}