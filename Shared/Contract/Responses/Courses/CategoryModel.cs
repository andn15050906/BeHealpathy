using Contract.Domain.CourseAggregate;
using System.Linq.Expressions;

namespace Contract.Responses.Courses;

public sealed class CategoryModel
{
    public Guid Id { get; set; }
    public string Path { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsLeaf { get; set; }
    public int CourseCount { get; set; }






    public static Expression<Func<Category, CategoryModel>> MapExpression
        = _ => new CategoryModel
        {
            Id = _.Id,
            Path = _.Path,
            Title = _.Title,
            Description = _.Description,
            IsLeaf = _.IsLeaf,
            CourseCount = _.CourseCount
        };
}