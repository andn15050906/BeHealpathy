using Contract.Domain.CommunityAggregate;
using Contract.Domain.CommunityAggregate.Enums;
using Contract.Responses.Shared;
using System.Linq.Expressions;

namespace Contract.Responses.Community;

public sealed class ChatMessageModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }

    public string Content { get; set; } = string.Empty;
    public MessageStatus Status { get; set; }
    public Guid ConversationId { get; set; }
    public IEnumerable<MultimediaModel> Attachments { get; set; } = [];
    public IEnumerable<ReactionModel> Reactions { get; set; } = [];






    public static Func<ChatMessage, ChatMessageModel> MapFunc
        = _ => new ChatMessageModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            LastModifierId = _.LastModifierId,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,

            Content = _.Content,
            Status = _.Status,
            ConversationId = _.ConversationId,
            //Attachments
            Reactions = _.Reactions?.Select(reaction => new ReactionModel
            {
                Id = reaction.Id,
                CreatorId = reaction.CreatorId,
                CreationTime = reaction.CreationTime,
                SourceId = reaction.SourceId,
                Content = reaction.Content
            }).ToList() ?? []
        };

    public static Expression<Func<ChatMessage, ChatMessageModel>> MapExpression
        = _ => new ChatMessageModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            LastModifierId = _.LastModifierId,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,

            Content = _.Content,
            Status = _.Status,
            ConversationId = _.ConversationId,
            //Attachments
            Reactions = _.Reactions.Select(reaction => new ReactionModel
            {
                Id = reaction.Id,
                CreatorId = reaction.CreatorId,
                CreationTime = reaction.CreationTime,
                SourceId = reaction.SourceId,
                Content = reaction.Content
            }).ToList(),
        };
}
