namespace Contract.Domain.ProgressAggregates;

public sealed class McqChoice : DomainObject
{
    // Keys
    public Guid SubmissionId { get; set; }
    public Guid McqQuestionId { get; set; }

    // Attributes
    public Guid McqAnswerId { get; set; }

    // Navigations
    public Submission Submission { get; set; }
    public McqQuestion Question { get; set; }




    public McqChoice()
    {

    }

    public McqChoice(Guid submissionId, Guid mcqQuestionId, Guid mcqAnswerId)
    {
        SubmissionId = submissionId;
        McqQuestionId = mcqQuestionId;

        McqAnswerId = mcqAnswerId;
    }
}