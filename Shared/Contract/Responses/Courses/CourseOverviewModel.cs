using Contract.Domain.CourseAggregate;
using Contract.Domain.CourseAggregate.Enums;
using System.Linq.Expressions;

namespace Contract.Responses.Courses;

public sealed class CourseOverviewModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }

    public string Title { get; set; }
    public string ThumbUrl { get; set; }
    //public string Intro { get; set; }
    //public string Description { get; set; }
    public CourseStatus Status { get; set; }
    public double Price { get; set; }
    public double Discount { get; set; }
    public DateTime DiscountExpiry { get; set; }
    public CourseLevel Level { get; set; }
    //public string Outcomes { get; set; }
    //public string Requirements { get; set; }
    public byte LectureCount { get; set; }
    public int LearnerCount { get; set; }
    public int RatingCount { get; set; }
    public long TotalRating { get; set; }

    public Guid LeafCategoryId { get; set; }
    public Guid InstructorId { get; set; }






    public static Expression<Func<Course, CourseOverviewModel>> MapExpression
        = _ => new CourseOverviewModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            LastModifierId = _.LastModifierId,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,

            Title = _.Title,
            ThumbUrl = _.ThumbUrl,
            //public string Intro { get; set; },
            //public string Description { get; set; },
            Status = _.Status,
            Price = _.Price,
            Discount = _.Discount,
            DiscountExpiry = _.DiscountExpiry,
            Level = _.Level,
            //public string Outcomes { get; set; },
            //public string Requirements { get; set; },
            LectureCount = _.LectureCount,
            LearnerCount = _.LearnerCount,
            RatingCount = _.RatingCount,
            TotalRating = _.TotalRating,
            LeafCategoryId = _.LeafCategoryId,
            InstructorId = _.InstructorId,
            //public UserMinModel Creator { get; set; }
        };
}
