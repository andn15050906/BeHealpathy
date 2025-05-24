using Contract.Domain.ProgressAggregate;
using System.Linq.Expressions;

namespace Contract.Responses.Progress;

public sealed class RoadmapModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }

    public string Title { get; set; }                           = string.Empty;
    public string IntroText { get; set; }                       = string.Empty;
    public string Description { get; set; }                     = string.Empty;
    public string Category { get; set; }                        = string.Empty;
    public string ThumbUrl { get; set; }                        = string.Empty;

    public double? Price { get; set; }
    public double? Discount { get; set; }
    public DateTime? DiscountExpiry { get; set; }
    public string? Coupons { get; set; }                        = string.Empty;

    public IEnumerable<RoadmapPhaseModel> Phases { get; set; }  = [];



    public static Expression<Func<Roadmap, RoadmapModel>> MapExpression
        = _ => new RoadmapModel
        {
            Id = _.Id,
            CreatorId = _.CreatorId,
            LastModifierId = _.LastModifierId,
            CreationTime = _.CreationTime,
            LastModificationTime = _.LastModificationTime,

            Title = _.Title,
            IntroText = _.IntroText,
            Description = _.Description,
            Category = _.Category,
            ThumbUrl = _.ThumbUrl,

            Price = _.Price,
            Discount = _.Discount,
            DiscountExpiry = _.DiscountExpiry,
            Coupons = _.Coupons,

            Phases = _.Phases.Select(_ => new RoadmapPhaseModel
            {
                Id = _.Id,

                Title = _.Title,
                Description = _.Description,
                Introduction = _.Introduction,
                Index = _.Index,
                TimeSpan = _.TimeSpan,
                IsRequiredToAdvance = _.IsRequiredToAdvance,
                QuestionsToAdvance = _.QuestionsToAdvance,
                VideoUrl = _.VideoUrl,

                Recommendations = _.Recommendations.Select(_ => new RoadmapRecommendationModel
                {
                    Id = _.Id,
                    RoadmapPhaseId = _.RoadmapPhaseId,
                    Title = _.Title,
                    Content = _.Content,
                    Description = _.Description,
                    IsAction = _.IsAction,
                    Duration = _.Duration,
                    MoodTags = _.MoodTags,
                    IsGeneralTip = _.IsGeneralTip,
                    Source = _.Source,
                    EntityType = _.EntityType,
                    TargetEntityId = _.TargetEntityId
                })
            })
        };
}
