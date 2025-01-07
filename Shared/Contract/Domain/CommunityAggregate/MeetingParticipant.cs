using Contract.Domain.CommunityAggregate.Enums;
using Contract.Domain.UserAggregate;

namespace Contract.Domain.CommunityAggregate;

public sealed class MeetingParticipant : CreationAuditedDomainObject
{
    // PK
    public Guid MeetingId { get; set; }
    public Guid UserId { get; set; }

    // Attributes
    public bool IsHost { get; set; }
    public RegistrationStatus Status { get; set; }

    // Navigations
    public User User { get; set; }



    public MeetingParticipant()
    {

    }

    public MeetingParticipant(Guid creatorId, Guid meetingId, bool isHost)
    {
        CreatorId = creatorId;
        MeetingId = meetingId;

        IsHost = isHost;
    }
}
