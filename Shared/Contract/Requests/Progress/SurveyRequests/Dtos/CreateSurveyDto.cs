using Contract.Requests.Progress.McqRequests.Dtos;

namespace Contract.Requests.Progress.SurveyRequests.Dtos;

public sealed class CreateSurveyDto
{
    public string Name { get; set; }
    public string Description { get; set; }

    public List<CreateMcqQuestionDto> Questions { get; set; }
    public List<CreateSurveyScoreBandDto>? Bands { get; set; }
}