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
    public string Intro { get; set; }
    public string Description { get; set; }
    // Status: Ongoing
    public double Price { get; set; }
    // Discount
    // DiscountExpiry
    public CourseLevel Level { get; set; }
    public string Outcomes { get; set; }
    public string Requirements { get; set; }

    public Guid LeafCategoryId { get; set; }
    public List<CreateLectureDto> Lectures { get; set; }
}

#pragma warning restore CS8618