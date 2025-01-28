using Contract.BusinessRules.PreferenceBiz;

namespace Contract.Requests.Identity;

public sealed class GetAllPreferenceSurveysQuery : IRequest<Result<List<PreferenceSurvey>>>
{
}