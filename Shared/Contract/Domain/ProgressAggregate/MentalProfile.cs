using Contract.Domain.UserAggregate;

namespace Contract.Domain.ProgressAggregate;

/// <summary>
/// Mental Profile (Attribute, Value)       - The current Mental Profile is the combination of latest attributes
/// 
/// Roadmap
///     Roadmap Phase
///         Roadmap Recommendation          - General recommendations from Advisor
///         Roadmap Milestone               - Generated and personalized based on user status and Roadmap Recommendation
///     
/// Submission
///     Survey
///         Survey Score Band
///         McqQuestion
///             McqAnswer
///     McqChoice
/// 
/// </summary>
public sealed class MentalProfile : CreationAuditedEntity
{
    public string Attribute { get; set; }               // MentalProfileConstants
    public string Value { get; set; }



#pragma warning disable CS8618
    public MentalProfile(Guid id, Guid creatorId, string attribute, string value)
    {
        Id = id;
        CreatorId = creatorId;
        Attribute = attribute;
        Value = value;
    }
#pragma warning restore CS8618
}