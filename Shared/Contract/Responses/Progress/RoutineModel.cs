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
    public Frequency Repeater { get; set; }
    public Guid? RepeaterSequenceId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsClosed { get; set; }
    public int Tag { get; set; }






    public static Func<Routine, RoutineModel> MapFunc
        = _ => new RoutineModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,

            Title = _.Title,
            Description = _.Description,
            Objective = _.Objective,
            Repeater = _.Repeater,
            RepeaterSequenceId = _.RepeaterSequenceId,
            StartDate = _.StartDate,
            EndDate = _.EndDate,
            IsCompleted = _.IsCompleted,
            IsClosed = _.IsClosed,
            Tag = _.Tag,
        };

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
            Repeater = _.Repeater,
            RepeaterSequenceId = _.RepeaterSequenceId,
            StartDate = _.StartDate,
            EndDate = _.EndDate,
            IsCompleted = _.IsCompleted,
            IsClosed = _.IsClosed,
            Tag = _.Tag,
        };
}
