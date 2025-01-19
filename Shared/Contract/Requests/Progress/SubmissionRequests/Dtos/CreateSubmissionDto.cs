namespace Contract.Requests.Progress.SubmissionRequests.Dtos;

public sealed class CreateSubmissionDto
{
    public Guid SurveyId { get; set; }

    public List<CreateMcqChoiceDto> Choices { get; set; }
}
