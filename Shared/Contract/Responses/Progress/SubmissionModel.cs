using Contract.Domain.ProgressAggregate;
using System.Linq.Expressions;

namespace Contract.Responses.Progress;

public sealed class SubmissionModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public DateTime CreationTime { get; set; }
    public Guid SurveyId { get; set; }
    public List<McqChoiceModel> Choices { get; set; }



    public static Expression<Func<Submission, SubmissionModel>> MapExpression
        = _ => new SubmissionModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            CreationTime = _.CreationTime,
            SurveyId = _.SurveyId,
            Choices = _.Choices.Select(_ => new McqChoiceModel
            {
                SubmissionId = _.SubmissionId,
                McqAnswerId = _.McqAnswerId
            }).ToList()
        };
}