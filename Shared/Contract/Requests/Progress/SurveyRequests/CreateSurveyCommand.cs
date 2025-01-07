using Contract.Requests.Progress.SurveyRequests.Dtos;

namespace Contract.Requests.Progress.SurveyRequests;

public sealed class CreateSurveyCommand : CreateCommand
{
    public CreateSurveyDto Rq { get; init; }
    public Guid UserId { get; init; }



    public CreateSurveyCommand(Guid id, CreateSurveyDto rq, Guid userId, bool isCompensating = false)
        : base(id, isCompensating)
    {
        Rq = rq;
        UserId = userId;
    }
}