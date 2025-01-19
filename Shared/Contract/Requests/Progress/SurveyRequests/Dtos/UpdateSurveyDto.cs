using Contract.Requests.Progress.McqRequests.Dtos;

namespace Contract.Requests.Progress.SurveyRequests.Dtos;

public sealed class UpdateSurveyDto
{
    public Guid Id { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }

    public List<CreateMcqQuestionDto>? AddedQuestions { get; set; }
    public List<Guid>? RemovedQuestions { get; set; }
}
