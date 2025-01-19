using Contract.Domain.ProgressAggregates;
using System.Linq.Expressions;

namespace Contract.Responses.Progress;

public sealed class SurveyModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<McqQuestionModel> Questions { get; set; }



    public static Expression<Func<Survey, SurveyModel>> MapExpression
       = _ => new SurveyModel
       {
           Id = _.Id,
           Name = _.Name,
           Description = _.Description,
           Questions = _.Questions.Select(_ => new McqQuestionModel
           {
               Id = _.Id,
               Content = _.Content,
               Explanation = _.Explanation,
               SurveyId = _.SurveyId,
               Answers = _.Answers.Select(_ => new McqAnswerModel
               {
                   Id = _.Id,
                   Content = _.Content
               }).ToList()
           }).ToList()
       };
}
