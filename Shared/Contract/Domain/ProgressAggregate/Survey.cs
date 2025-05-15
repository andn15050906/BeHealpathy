namespace Contract.Domain.ProgressAggregate;

public sealed class Survey : Entity
{
    // Attributes
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsScientific { get; set; }

    // Navigations
    public List<McqQuestion> Questions { get; set; }
    public List<SurveyScoreBand> Bands { get; set; }



#pragma warning disable 
    public Survey()
    {

    }

    public Survey(Guid id, string name, string description, bool isScientific, List<McqQuestion> questions, List<SurveyScoreBand> bands)
    {
        Id = id;

        Name = name;
        Description = description;
        IsScientific = isScientific;

        Questions = questions;
        Bands = bands;
    }
#pragma warning restore CS8618
}