using Contract.Domain.ToolAggregate.Enums;
using Contract.Domain.UserAggregate;

namespace Contract.Domain.ToolAggregate;

public sealed class Routine : AuditedEntity
{
    // Attributes
    public string Title { get; set; }
    public string Description { get; set; }
    public string Objective { get; set; }
    public Frequency Repeater { get; set; }
    public Guid? RepeaterSequenceId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsClosed { get; set; }
    public int Tag { get; set; }

    // Navigations
    public List<RoutineLog> Logs { get; set; }
    public User Creator { get; set; }



#pragma warning disable CS8618
    public Routine()
    {

    }

    public Routine(
        Guid id, Guid creatorId, string title, string description, string objective, Frequency repeater,
        Guid? repeaterSequenceId, DateTime startDate, DateTime endDate, bool isCompleted, bool isClosed, int tag)
    {
        Id = id;
        CreatorId = creatorId;

        Title = title;
        Description = description;
        Objective = objective;
        Repeater = repeater;
        RepeaterSequenceId = repeaterSequenceId;
        StartDate = startDate;
        EndDate = endDate;
        IsCompleted = isCompleted;
        IsClosed = isClosed;
        Tag = tag;
    }
#pragma warning restore CS8618
}
