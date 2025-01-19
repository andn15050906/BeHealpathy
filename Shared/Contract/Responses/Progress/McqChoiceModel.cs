using Contract.Domain.ProgressAggregates;
using System.Linq.Expressions;

namespace Contract.Responses.Progress;

public sealed class McqChoiceModel
{
    public Guid SubmissionId { get; set; }
    public Guid McqQuestionId { get; set; }
    public Guid McqAnswerId { get; set; }



    public static Expression<Func<McqChoice, McqChoiceModel>> MapExpression
       = _ => new McqChoiceModel
       {
           SubmissionId = _.SubmissionId,
           McqQuestionId = _.McqQuestionId,
           McqAnswerId = _.McqAnswerId
       };
}
