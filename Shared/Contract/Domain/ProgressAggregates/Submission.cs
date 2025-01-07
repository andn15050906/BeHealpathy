namespace Contract.Domain.ProgressAggregates;


public sealed class Submission : AuditedEntity
{
    // Navigations
    public List<McqChoice> Choices { get; set; }






#pragma warning disable CS8618
    public Submission()
    {

    }
#pragma warning restore CS8618

    public Submission(Guid id, Guid creatorId)
    {
        Id = id;
        CreatorId = creatorId;
    }
}