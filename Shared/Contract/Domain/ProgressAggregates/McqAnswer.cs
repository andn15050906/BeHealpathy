namespace Contract.Domain.ProgressAggregates;

// not a real entity
public sealed class McqAnswer : Entity
{
    // Attributes
    public string Content { get; set; }
    public int Score { get; set; }
    // IsCorrect

#pragma warning disable CS8618
    public McqAnswer()
    {

    }
#pragma warning restore CS8618

    public McqAnswer(Guid id, string content, int score)
    {
        Id = id;
        Content = content;
        Score = score;
    }
}
