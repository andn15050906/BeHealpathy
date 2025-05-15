namespace Contract.Domain.ProgressAggregate;

public sealed class McqChoice : DomainObject
{
    // Keys
    public Guid SubmissionId { get; set; }
    public Guid McqAnswerId { get; set; }

    // Navigations
    public Submission Submission { get; set; }
    public McqAnswer Answer { get; set; }



#pragma warning disable CS8618
    public McqChoice()
    {

    }

    public McqChoice(Guid submissionId, Guid mcqAnswerId)
    {
        SubmissionId = submissionId;
        McqAnswerId = mcqAnswerId;
    }
#pragma warning restore CS8618
}