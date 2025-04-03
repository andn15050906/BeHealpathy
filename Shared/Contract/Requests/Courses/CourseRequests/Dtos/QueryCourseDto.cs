using Contract.Domain.CourseAggregate.Enums;
using Contract.Requests.Shared.Base;

namespace Contract.Requests.Courses.CourseRequests.Dtos;

public sealed class QueryCourseDto : PagingQueryDto
{
    public string? Title { get; set; }
    public CourseStatus? Status { get; set; }
    public CourseLevel? Level { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? InstructorId { get; set; }
    public Guid? CreatorId { get; set; }

    // Single
    //public Guid SectionId { get; set; }

    // Order By
    public bool ByPrice { get; set; }
    public bool ByDiscount { get; set; }
    public bool ByLearnerCount { get; set; }
    public bool ByAvgRating { get; set; }

    // Complicated Order By
    public bool ByTrend { get; set; }
}
