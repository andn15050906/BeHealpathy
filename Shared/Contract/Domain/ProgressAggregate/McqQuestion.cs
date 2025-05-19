namespace Contract.Domain.ProgressAggregate;

public sealed class McqQuestion : Entity
{
    // Attributes
    public string Content { get; set; }
    public string? Precondition { get; set; }
    public int Index { get; set; }

    // FKs
    public Guid SurveyId { get; set; }

    // Navigations
    public List<McqAnswer> Answers { get; set; }



#pragma warning disable CS8618
    public McqQuestion()
    {

    }
#pragma warning restore CS8618

    public McqQuestion(Guid id, string content, string? precondition, int index, Guid surveyId, List<McqAnswer> choices)
    {
        Id = id;

        Content = content;
        Precondition = precondition;
        Index = index;

        SurveyId = surveyId;
        Answers = choices;
    }
}