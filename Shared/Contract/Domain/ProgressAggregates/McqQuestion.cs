namespace Contract.Domain.ProgressAggregates;

public sealed class McqQuestion : Entity
{
    // Attributes
    public string Content { get; set; }
    public string Explanation { get; set; }
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

    public McqQuestion(Guid id, string content, string explanation, int index, Guid surveyId, List<McqAnswer> choices)
    {
        Id = id;

        Content = content;
        Explanation = explanation;
        Index = index;

        SurveyId = surveyId;
        Answers = choices;
    }
}
