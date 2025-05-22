using Contract.Domain.CourseAggregate.Enums;
using Contract.Domain.UserAggregate;

namespace Contract.Domain.CourseAggregate;

/// <summary>
/// User's enrollment and Course progress
/// </summary>
public sealed class CourseProgress : AuditedEntity
{
    // Part of PK
    public Guid CourseId { get; set; }

    // Attributes
    public EnrollmentStatus Status { get; set; }
    public int CurrentIndex { get; set; }                   // current Lecture (not saving LectureId)
    public string Outcome { get; set; }                     // User input
    public DateTime ExpectedCompletionDate { get; set; }    // Calculated

    // FKs
    public Guid? BillId { get; set; }

    // Navigations
    public Course Course { get; set; }
    public User Creator { get; set; }



#pragma warning disable CS8618
    public CourseProgress()
    {

    }

    public CourseProgress(Guid id, Guid courseId, Guid creatorId, Guid billId)
    {
        Id = id;
        CourseId = courseId;
        CreatorId = creatorId;
        BillId = billId;

        Outcome = string.Empty;
    }

    public CourseProgress(Guid id, Guid courseId, Guid creatorId)
    {
        Id = id;
        CourseId = courseId;
        CreatorId = creatorId;

        Outcome = string.Empty;
    }
#pragma warning restore CS8618
}