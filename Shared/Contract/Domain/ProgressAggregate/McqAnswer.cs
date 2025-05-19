namespace Contract.Domain.ProgressAggregate;

// not a true entity
public sealed class McqAnswer : Entity
{
    // Attributes
    public string Content { get; set; }
    public string? OptionValue { get; set; }
    public int Score { get; set; }
    public int Index { get; set; }

#pragma warning disable CS8618
    public McqAnswer()
    {

    }
#pragma warning restore CS8618

    public McqAnswer(Guid id, string content, string? optionValue, int score)
    {
        Id = id;
        Content = content;
        OptionValue = optionValue;
        Score = score;
    }
}