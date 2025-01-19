using Contract.Requests.Progress.SurveyRequests.Dtos;

namespace Contract.Requests.Progress.SurveyRequests;

public sealed class UpdateSurveyCommand : UpdateCommand
{
    public UpdateSurveyDto Rq { get; init; }
    public Guid UserId { get; init; }



    public UpdateSurveyCommand(UpdateSurveyDto rq, Guid userId, bool isCompensating = false)
        : base(isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}