using Contract.Domain.CourseAggregate;
using System.Linq.Expressions;

namespace Contract.Responses.Courses;

public sealed class CourseMinModel
{
    public Guid Id { get; private set; }
    public DateTime LastModificationTime { get; set; }

    public string Title { get; private set; }
    public string ThumbUrl { get; set; }
    public double Price { get; set; }
    public double Discount { get; private set; }
    public DateTime DiscountExpiry { get; private set; }






    public static Expression<Func<Course, CourseMinModel>> MapExpression
        = _ => new CourseMinModel
        {
            Id = _.Id,
            LastModificationTime = _.LastModificationTime,
            Title = _.Title,
            ThumbUrl = _.ThumbUrl,
            Price = _.Price,
            Discount = _.Discount,
            DiscountExpiry = _.DiscountExpiry,
        };
}
