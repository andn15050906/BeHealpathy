using Contract.Domain.ProgressAggregate;
using System.Linq.Expressions;

namespace Contract.Responses.Progress;

public sealed class McqAnswerModel
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public string? OptionValue { get; set; }
    public int Score { get; set; }



    public static Expression<Func<McqAnswer, McqAnswerModel>> MapExpression
        = _ => new McqAnswerModel
        {
            Id = _.Id,
            Content = _.Content,
            OptionValue = _.OptionValue,
            Score = _.Score
        };
}