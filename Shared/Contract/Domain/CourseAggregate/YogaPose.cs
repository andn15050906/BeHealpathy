using Contract.Domain.UserAggregate;

namespace Contract.Domain.CourseAggregate
{
    public sealed class YogaPose : AuditedEntity
    {
        public string Name { get; set; }
        public string EmbeddedUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string? Description { get; set; }
        public string Level { get; set; }
        public string? EquipmentRequirement { get; set; }
        public string? ThumpUrl { get; set; }
        public User Creator { get; set; }

#pragma warning disable CS8618

        public YogaPose()
        {
        }

        public YogaPose(Guid id,
                        Guid creatorId,
                        string name,
                        string embeddedUrl,
                        string? videoUrl,
                        string? description,
                        string level,
                        string? equipmentRequirement,
                        string? thumpUrl)
        {
            Id = id;
            CreatorId = creatorId;
            Name = name;
            EmbeddedUrl = embeddedUrl;
            VideoUrl = videoUrl;
            Description = description;
            Level = level;
            EquipmentRequirement = equipmentRequirement;
            ThumpUrl = thumpUrl;
        }

#pragma warning restore CS8618
    }
}