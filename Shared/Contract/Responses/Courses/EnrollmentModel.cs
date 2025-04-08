using System.Linq.Expressions;
using Contract.Domain.CourseAggregate;
using Contract.Domain.CourseAggregate.Enums;

namespace Contract.Responses.Courses;

public class EnrollmentModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }

    public EnrollmentStatus Status { get; set; }

    public Guid? BillId { get; set; }

    public Guid CourseId { get; set; }






    public static Expression<Func<Enrollment, EnrollmentModel>> MapExpression
        = _ => new EnrollmentModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,

            Status = _.Status,
            BillId = _.BillId,

            CourseId = _.CourseId
        };
}
