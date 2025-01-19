using Contract.Domain.CourseAggregate.Enums;
using Contract.Domain.UserAggregate;
using Core.Domain;

namespace Contract.Domain.CourseAggregate;

// Aggregate Root
public sealed class Enrollment : AuditedEntity
{
    // Part of PK
    public Guid CourseId { get; set; }

    // Attributes
    public EnrollmentStatus Status { get; set; }

    // FKs
    public Guid? BillId { get; set; }

    // Navigations
    public Course Course { get; set; }
    public User Creator { get; set; }



    public Enrollment()
    {

    }

    public Enrollment(Guid id, Guid courseId, Guid creatorId, Guid billId)
    {
        Id = id;
        CourseId = courseId;
        CreatorId = creatorId;
        BillId = billId;
    }

    public Enrollment(Guid id, Guid courseId, Guid creatorId)
    {
        Id = id;
        CourseId = courseId;
        CreatorId = creatorId;
    }
}