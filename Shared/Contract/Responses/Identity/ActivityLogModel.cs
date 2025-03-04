namespace Contract.Responses.Identity;

public sealed class ActivityLogModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public DateTime CreationTime { get; set; }

    public string Content { get; set; }
}