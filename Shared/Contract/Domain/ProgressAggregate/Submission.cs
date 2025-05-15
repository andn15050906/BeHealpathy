using Contract.Domain.UserAggregate;

namespace Contract.Domain.ProgressAggregate;

public sealed class Submission : AuditedEntity
{
    public Guid SurveyId { get; set; }

    // Navigations
    public Survey Survey { get; set; }
    public List<McqChoice> Choices { get; set; }
    public User Creator { get; set; }






#pragma warning disable CS8618
    public Submission()
    {

    }

    public Submission(Guid id, Guid creatorId, Guid surveyId, List<McqChoice> choices)
    {
        Id = id;
        CreatorId = creatorId;
        SurveyId = surveyId;
        Choices = choices;
    }
#pragma warning restore CS8618
}