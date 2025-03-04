using Contract.BusinessRules.PreferenceBiz;

namespace Contract.Requests.Identity.PreferenceRequests;

public sealed class GetAllPreferenceSurveysQuery : IRequest<Result<List<PreferenceSurvey>>>
{
}