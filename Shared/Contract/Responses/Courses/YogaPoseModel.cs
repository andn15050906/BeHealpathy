using Contract.Domain.CourseAggregate;
using System.Linq.Expressions;

namespace Contract.Responses.Courses;

public sealed class YogaPoseModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }

    public string? Name { get; set; }
    public string? EmbeddedUrl { get; set; }
    public string? VideoUrl { get; set; }
    public string? Description { get; set; }
    public string? Level { get; set; }
    public string? EquipmentRequirement { get; set; }
    public string? ThumpUrl { get; set; }

    public static Expression<Func<YogaPose, YogaPoseModel>> MapExpression = _ => new YogaPoseModel
    {
        Id = _.Id,
        CreatorId = _.CreatorId,
        LastModifierId = _.LastModifierId,
        CreationTime = _.CreationTime,
        LastModificationTime = _.LastModificationTime,

        Name = _.Name,
        Level = _.Level,
        Description = _.Description,
        EmbeddedUrl = _.EmbeddedUrl,
        VideoUrl = _.VideoUrl,
        EquipmentRequirement = _.EquipmentRequirement,
        ThumpUrl = _.ThumpUrl 
    };
}