using Contract.Domain.UserAggregate;
using System.Linq.Expressions;

namespace Contract.Responses.Identity;

public sealed class ActivityLogModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public DateTime CreationTime { get; set; }

    public string Content { get; set; }



    public static Expression<Func<ActivityLog, ActivityLogModel>> MapExpression
        => _ => new ActivityLogModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            CreationTime = _.CreationTime,
            Content = _.Content
        };
}