using Contract.Domain.CommunityAggregate;
using System.Linq.Expressions;

namespace Contract.Responses.Community;

public sealed class MeetingModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public int MaxParticipants { get; set; }
    public List<MeetingParticipantModel> Participants { get; set; }






    public static Func<Meeting, MeetingModel> MapFunc
        = _ => new MeetingModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            LastModifierId = _.LastModifierId,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,

            Title = _.Title,
            Description = _.Description,
            StartAt = _.StartAt,
            EndAt = _.EndAt,
            MaxParticipants = _.MaxParticipants,

            Participants = _.Participants.Select(_ => new MeetingParticipantModel
            {
                CreatorId = _.CreatorId,
                CreationTime = _.CreationTime,
                MeetingId = _.MeetingId,
                UserId = _.UserId,
                IsHost = _.IsHost,
                Status = _.Status
            }).ToList()
        };

    public static Expression<Func<Meeting, MeetingModel>> MapExpression
        = _ => new MeetingModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            LastModifierId = _.LastModifierId,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,

            Title = _.Title,
            Description = _.Description,
            StartAt = _.StartAt,
            EndAt = _.EndAt,
            MaxParticipants = _.MaxParticipants,

            Participants = _.Participants.Select(_ => new MeetingParticipantModel
            {
                CreatorId = _.CreatorId,
                CreationTime = _.CreationTime,
                MeetingId = _.MeetingId,
                UserId = _.UserId,
                IsHost = _.IsHost,
                Status = _.Status
            }).ToList()
        };
}
