using Contract.Domain.ProgressAggregate;
using System.Linq.Expressions;

namespace Contract.Responses.Progress;

public sealed class McqChoiceModel
{
    public Guid SubmissionId { get; set; }
    public Guid McqAnswerId { get; set; }



    public static Expression<Func<McqChoice, McqChoiceModel>> MapExpression
       = _ => new McqChoiceModel
       {
           SubmissionId = _.SubmissionId,
           McqAnswerId = _.McqAnswerId
       };
}
