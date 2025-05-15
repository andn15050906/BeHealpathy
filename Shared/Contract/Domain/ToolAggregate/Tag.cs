namespace Contract.Domain.ToolAggregate;

public sealed class Tag : Entity
{
    // Attributes
    public string Title { get; set; }






#pragma warning disable CS8618
    public Tag()
    {

    }

    public Tag(string title)
    {
        Title = title;
    }
#pragma warning restore CS8618
}
