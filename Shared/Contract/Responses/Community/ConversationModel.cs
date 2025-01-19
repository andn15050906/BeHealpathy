using Contract.Domain.CommunityAggregate;
using System.Linq.Expressions;

namespace Contract.Responses.Community;

public sealed class ConversationModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public DateTime CreationTime { get; set; }

    public string Title { get; set; }
    public bool IsPrivate { get; set; }
    public string AvatarUrl { get; set; }
    public List<ConversationMemberModel> Members { get; set; }



    public static Expression<Func<Conversation, ConversationModel>> MapExpression
        = _ => new ConversationModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            CreationTime = _.CreationTime,
            Title = _.Title,
            IsPrivate = _.IsPrivate,
            AvatarUrl = _.AvatarUrl,
            Members = _.Members.Select(_ => new ConversationMemberModel
            {
                CreatorId = _.CreatorId,
                CreationTime = _.CreationTime,
                ConversationId = _.ConversationId,
                IsAdmin = _.IsAdmin,
                LastVisit = _.LastVisit
            }).ToList()
        };
}
