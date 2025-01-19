using Contract.Domain.CommunityAggregate;
using Contract.Domain.CommunityAggregate.Enums;
using System.Linq.Expressions;

namespace Contract.Responses.Community;

public sealed class MeetingParticipantModel
{
    public Guid CreatorId { get; set; }
    public DateTime CreationTime { get; set; }

    public Guid MeetingId { get; set; }
    public Guid UserId { get; set; }
    public bool IsHost { get; set; }
    public RegistrationStatus Status { get; set; }



    public static Expression<Func<MeetingParticipant, MeetingParticipantModel>> MapExpression
        = _ => new MeetingParticipantModel
        {
            CreatorId = _.CreatorId,
            CreationTime = _.CreationTime,
            MeetingId = _.MeetingId,
            UserId = _.UserId,
            IsHost = _.IsHost,
            Status = _.Status
        };
}
