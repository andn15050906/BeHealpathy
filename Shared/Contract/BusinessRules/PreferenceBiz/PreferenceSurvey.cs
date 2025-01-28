namespace Contract.BusinessRules.PreferenceBiz;

public class PreferenceSurvey
{
    public Guid Id { get; }
    public string Title { get; }
    public Dictionary<Guid, string> Values { get; }

    public PreferenceSurvey(Guid id, string title, Dictionary<Guid, string> values)
    {
        Id = id;
        Title = title;
        Values = values;
    }
}