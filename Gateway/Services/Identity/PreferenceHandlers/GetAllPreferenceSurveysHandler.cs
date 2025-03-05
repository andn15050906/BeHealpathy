using Contract.BusinessRules.PreferenceBiz;
using Contract.Helpers;
using Contract.Requests.Identity.PreferenceRequests;

namespace Gateway.Services.Identity.PreferenceHandlers;

public class GetAllPreferenceSurveysHandler : RequestHandler<GetAllPreferenceSurveysQuery, List<PreferenceSurvey>, HealpathyContext>
{
    public GetAllPreferenceSurveysHandler(HealpathyContext context, IAppLogger logger) : base(context, logger)
    {
    }

    public override Task<Result<List<PreferenceSurvey>>> Handle(GetAllPreferenceSurveysQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Ok(PrefStore.Surveys));
    }
}