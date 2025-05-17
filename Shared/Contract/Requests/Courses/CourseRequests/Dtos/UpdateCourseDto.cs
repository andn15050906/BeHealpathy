using Contract.Domain.CourseAggregate.Enums;
using Contract.Requests.Shared.BaseDtos.Media;

namespace Contract.Requests.Courses.CourseRequests.Dtos;

public sealed class UpdateCourseDto
{
    public Guid Id { get; init; }
    public Guid? LeafCategoryId { get; init; }

    public string? Title { get; init; }
    public CreateMediaDto? Thumb { get; init; }
    public string? Intro { get; init; }
    public string? Description { get; init; }
    public CourseStatus? Status { get; init; }

    public double? Price { get; init; }
    public double? Discount { get; init; }
    public DateTime? DiscountExpiry { get; init; }

    public CourseLevel? Level { get; init; }
    public string? AdvisorExpectedOutcome { get; init; }
    public string? Outcomes { get; init; }
    public string? Requirements { get; init; }
    public int? ExpectedCompletion { get; init; }
}
