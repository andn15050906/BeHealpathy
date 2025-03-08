using Contract.Domain.ProgressAggregates;
using System.Linq.Expressions;

namespace Contract.Responses.Progress;

public sealed class RoadmapModel
{
    public Guid Id { get; set; }
    public Guid CreatorId { get; set; }
    public Guid LastModifierId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime LastModificationTime { get; set; }



    public string Title { get; set; }
    public string IntroText { get; set; }
    public IEnumerable<RoadmapPhaseModel> Phases { get; set; }



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
            Phases = _.Phases.Select(_ => new RoadmapPhaseModel
            {
                Id = _.Id,
                Title = _.Title,
                Description = _.Description,
                Milestones = _.Milestones.Select(_ => new RoadmapMilestoneModel
                {
                    Id = _.Id,
                    Title = _.Title,
                    EventName = _.EventName,
                    RepeatTimesRequired = _.RepeatTimesRequired,
                    TimeSpentRequired = _.TimeSpentRequired,
                    Recommendations = _.Recommendations.Select(_ => new RoadmapRecommendationModel
                    {
                        Id = _.Id,
                        EntityType = _.EntityType,
                        MilestoneId = _.MilestoneId,
                        TargetEntityId = _.TargetEntityId,
                        Trait = _.Trait,
                        TraitDescription = _.TraitDescription
                    })
                })
            })
        };
}
