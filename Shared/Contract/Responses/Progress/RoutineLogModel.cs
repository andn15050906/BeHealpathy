using Contract.Domain.ProgressAggregates;
using System.Linq.Expressions;

namespace Contract.Responses.Progress;

public sealed class RoutineLogModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public DateTime CreationTime { get; set; }
    public Guid RoutineId { get; set; }
    public string Content { get; set; }



    public static Expression<Func<RoutineLog, RoutineLogModel>> MapExpression
       = _ => new RoutineLogModel
       {
           Id = _.Id,
           CreatorId = _.CreatorId,
           CreationTime = _.CreationTime,
           RoutineId = _.RoutineId,
           Content = _.Content
       };
}