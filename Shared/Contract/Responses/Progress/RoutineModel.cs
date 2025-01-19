using Contract.Domain.ProgressAggregates;
using Contract.Domain.ProgressAggregates.Enums;
using System.Linq.Expressions;

namespace Contract.Responses.Progress;

public sealed class RoutineModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public string Objective { get; set; }
    public Frequency Frequency { get; set; }



    public static Expression<Func<Routine, RoutineModel>> MapExpression
        = _ => new RoutineModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,
            Title = _.Title,
            Description = _.Description,
            Objective = _.Objective,
            Frequency = _.Frequency
        };
}
