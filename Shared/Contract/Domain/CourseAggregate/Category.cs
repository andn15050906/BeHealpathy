using Core.Domain;
using Contract.BusinessRules.Messaging;

namespace Contract.Domain.CourseAggregate;

public sealed class Category : Entity
{
    // Attributes
    public string Path { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsLeaf { get; set; }
    public int CourseCount { get; set; }






#pragma warning disable CS8618
    public Category()
    {

    }
#pragma warning restore CS8618

    public Category(Guid id, string title, string description, bool isLeaf, Category? parent = null)
    {
        if (parent is not null && parent.IsLeaf)
            throw new Exception(BusinessMessages.Category.INVALID_PARENT);

        Id = id;

        Title = title;
        Description = description;
        IsLeaf = isLeaf;
        CourseCount = 0;
        Path = parent is not null ? $"{parent.Path}_{Id}" : Id.ToString();
    }

    public void SetPath(Category parent)
    {
        Path = $"{parent.Path}_{Id}";
    }
}