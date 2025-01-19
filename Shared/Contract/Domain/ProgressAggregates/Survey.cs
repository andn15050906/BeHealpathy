namespace Contract.Domain.ProgressAggregates;

public sealed class Survey : Entity
{
    // Attributes
    public string Name { get; set; }
    public string Description { get; set; }

    // Navigations
    public List<McqQuestion> Questions { get; set; }



#pragma warning disable 
    public Survey()
    {

    }

    public Survey(Guid id, string name, string description, List<McqQuestion> questions)
    {
        Id = id;
        Name = name;
        Description = description;
        Questions = questions;
    }
#pragma warning restore CS8618
}
