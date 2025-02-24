using Contract.Domain.ProgressAggregates;
using System.Linq.Expressions;

namespace Contract.Responses.Progress;

public sealed class McqAnswerModel
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public int Score { get; set; }



    public static Expression<Func<McqAnswer, McqAnswerModel>> MapExpression
        = _ => new McqAnswerModel
        {
            Id = _.Id,
            Content = _.Content,
            Score = _.Score
        };
}