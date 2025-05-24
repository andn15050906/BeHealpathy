using System.Linq.Expressions;
using Contract.Domain.CourseAggregate;
using Contract.Domain.CourseAggregate.Enums;

namespace Contract.Responses.Courses;

public class CourseProgressModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }

    public EnrollmentStatus Status { get; set; }
    public int CurrentIndex { get; set; }                   // current Lecture (not saving LectureId)
    public string Outcome { get; set; }                     // User input
    public DateTime ExpectedCompletionDate { get; set; }    // Calculated

    public Guid? BillId { get; set; }
    public Guid CourseId { get; set; }







    public static Expression<Func<CourseProgress, CourseProgressModel>> MapExpression
        = _ => new CourseProgressModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,

            Status = _.Status,
            CurrentIndex = _.CurrentIndex,
            Outcome = _.Outcome,
            ExpectedCompletionDate = _.ExpectedCompletionDate,

            BillId = _.BillId,
            CourseId = _.CourseId
        };
}
