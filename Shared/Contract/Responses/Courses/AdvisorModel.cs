using System.Linq.Expressions;
using Contract.Domain.CourseAggregate;

namespace Contract.Responses.Courses;

public sealed class AdvisorModel
{
    public Guid Id { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }

    public string Intro { get; set; }
    public string Experience { get; set; }
    public long Balance { get; set; }
    public byte CourseCount { get; set; }






    public static Expression<Func<Advisor, AdvisorModel>> MapExpression
        = _ => new AdvisorModel
        {
            Id = _.Id,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,
            Intro = _.Intro,
            Experience = _.Experience,
            Balance = _.Balance,
            CourseCount = _.CourseCount
        };
}
