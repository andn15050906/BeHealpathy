namespace Contract.Requests.Progress.McqRequests.Dtos;

public sealed class CreateMcqQuestionDto
{
    public Guid SurveyId { get; set; }

    public string Content { get; set; }
    public string Explanation { get; set; }

    public List<CreateMcqAnswerDto> Answers { get; set; }
}
