using Contract.Domain.CommunityAggregate.Enums;
using Contract.Domain.UserAggregate;

namespace Contract.Domain.CommunityAggregate;

public sealed class MeetingParticipant : CreationAuditedDomainObject
{
    public Guid MeetingId { get; set; }

    // Attributes
    public bool IsHost { get; set; }
    public RegistrationStatus Status { get; set; }

    // Navigations
    public User Creator { get; set; }                       // Creator != Meeting Creator



#pragma warning disable CS8618
    public MeetingParticipant()
    {

    }

    public MeetingParticipant(Guid creatorId, Guid meetingId, bool isHost)
    {
        CreatorId = creatorId;
        MeetingId = meetingId;

        IsHost = isHost;
    }
#pragma warning restore CS8618
}
