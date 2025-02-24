using Contract.Domain.ProgressAggregates;
using System.Linq.Expressions;

namespace Contract.Responses.Progress;

public sealed class McqQuestionModel
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public string Explanation { get; set; }
    public int Index { get; set; }
    public Guid SurveyId { get; set; }
    public List<McqAnswerModel> Answers { get; set; }



    public static Expression<Func<McqQuestion, McqQuestionModel>> MapExpression
        = _ => new McqQuestionModel
        {
            Id = _.Id,
            Content = _.Content,
            Explanation = _.Explanation,
            Index = _.Index,
            SurveyId = _.SurveyId,
            Answers = _.Answers.Select(_ => new McqAnswerModel
            {
                Id = _.Id,
                Content = _.Content,
                Score = _.Score
            }).ToList()
        };
}
