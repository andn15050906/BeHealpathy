namespace Contract.Domain.ProgressAggregates;

public sealed class Survey : Entity
{
    // Attributes
    public string Name { get; set; }
    public string Description { get; set; }

    // Navigations
    public List<McqQuestion> Questions { get; set; }
}
