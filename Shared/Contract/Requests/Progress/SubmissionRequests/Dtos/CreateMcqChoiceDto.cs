namespace Contract.Requests.Progress.SubmissionRequests.Dtos;

public sealed class CreateMcqChoiceDto
{
    public Guid McqQuestionId { get; set; }
    public Guid McqAnswerId { get; set; }
}
