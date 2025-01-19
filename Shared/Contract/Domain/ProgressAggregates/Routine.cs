using Contract.Domain.ProgressAggregates.Enums;
using Contract.Domain.UserAggregate;

namespace Contract.Domain.ProgressAggregates;

public sealed class Routine : AuditedEntity
{
    // Attributes
    public string Title { get; set; }
    public string Description { get; set; }
    public string Objective { get; set; }
    public Frequency Frequency { get; set; }

    // Navigations
    public List<RoutineLog> Logs { get; set; }
    public User Creator { get; set; }



#pragma warning disable CS8618
    public Routine()
    {

    }

    public Routine(Guid id, Guid creatorId, string title, string description, Frequency frequency)
    {
        Id = id;
        CreatorId = creatorId;
        Title = title;
        Description = description;
        Frequency = frequency;
    }
#pragma warning restore CS8618
}
