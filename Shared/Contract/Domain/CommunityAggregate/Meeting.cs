using Contract.Domain.UserAggregate;

namespace Contract.Domain.CommunityAggregate;

public sealed class Meeting : AuditedEntity
{
    // Attributes
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public int MaxParticipants { get; set; }

    // Navigations
    public List<MeetingParticipant> Participants { get; set; }
    public User Creator { get; set; }



#pragma warning disable CS8618
    public Meeting()
    {

    }

    public Meeting(
        Guid id, Guid creatorId, string title, string description,
        DateTime startAt, DateTime endAt, int maxParticipants, List<MeetingParticipant> participants)
    {
        Id = id;
        CreatorId = creatorId;

        Title = title;
        Description = description;
        StartAt = startAt;
        EndAt = endAt;
        MaxParticipants = maxParticipants;
        Participants = participants;
    }
#pragma warning restore CS8618
}
