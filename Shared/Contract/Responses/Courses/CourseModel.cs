using System.Linq.Expressions;
using Contract.Domain.CourseAggregate;
using Contract.Domain.CourseAggregate.Enums;

namespace Contract.Responses.Courses;

public sealed class CourseModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }

    public string Title { get; set; }
    public string MetaTitle { get; set; }
    public string ThumbUrl { get; set; }
    public string Intro { get; set; }
    public string Description { get; set; }
    public CourseStatus Status { get; set; }
    public double Price { get; set; }
    public double Discount { get; set; }
    public DateTime DiscountExpiry { get; set; }
    public CourseLevel Level { get; set; }
    public string Outcomes { get; set; }
    public string Requirements { get; set; }
    public byte LectureCount { get; set; }
    public int LearnerCount { get; set; }
    public int RatingCount { get; set; }
    public long TotalRating { get; set; }

    public Guid LeafCategoryId { get; set; }
    public Guid InstructorId { get; set; }






    public static Expression<Func<Course, CourseModel>> MapExpression
        = _ => new CourseModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            LastModifierId = _.LastModifierId,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,

            Title = _.Title,
            MetaTitle = _.MetaTitle,
            ThumbUrl = _.ThumbUrl,
            Intro = _.Intro,
            Description = _.Description,
            Status = _.Status,
            Price = _.Price,
            Discount = _.Discount,
            DiscountExpiry = _.DiscountExpiry,
            Level = _.Level,
            Outcomes = _.Outcomes,
            Requirements = _.Requirements,
            LectureCount = _.LectureCount,
            LearnerCount = _.LearnerCount,
            RatingCount = _.RatingCount,
            TotalRating = _.TotalRating,

            LeafCategoryId = _.LeafCategoryId,
            InstructorId = _.InstructorId
        };
}
