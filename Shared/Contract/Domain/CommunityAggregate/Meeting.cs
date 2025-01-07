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
}
