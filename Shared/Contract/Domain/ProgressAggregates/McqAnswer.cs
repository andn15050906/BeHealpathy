namespace Contract.Domain.ProgressAggregates;

public sealed class McqAnswer : Entity
{
    // Attributes
    public string Content { get; set; }

#pragma warning disable CS8618
    public McqAnswer()
    {

    }
#pragma warning restore CS8618

    public McqAnswer(string content)
    {
        Content = content;
    }
}
