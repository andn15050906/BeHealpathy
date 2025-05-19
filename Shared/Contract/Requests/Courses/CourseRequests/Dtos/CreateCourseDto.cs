using Contract.Domain.CourseAggregate.Enums;
using Contract.Requests.Courses.LectureRequests.Dtos;
using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Courses.CourseRequests.Dtos;

#pragma warning disable CS8618

public sealed class CreateCourseDto
{
    public string Title { get; set; }
    // MetaTitle: generated
    public CreateMediaDto Thumb { get; set; }
    public string? Intro { get; set; }                      = string.Empty;
    public string? Description { get; set; }                = string.Empty;
    public CourseStatus Status { get; set; }                = CourseStatus.Published;

    public double? Price { get; set; }                      = 0;
    public double? Discount { get; set; }                   = 0;
    public DateTime? DiscountExpiry { get; set; }

    public CourseLevel Level { get; set; }
    public string? AdvisorExpectedOutcome { get; set; }     = string.Empty;
    public string? Outcomes { get; set; }                   = string.Empty;
    public string? Requirements { get; set; }               = string.Empty;
    public string? Tags { get; set; }                       = string.Empty;
    public int? ExpectedCompletion { get; set; }

    public Guid LeafCategoryId { get; set; }
    public List<CreateLectureDto>? Lectures { get; set; }
}

#pragma warning restore CS8618