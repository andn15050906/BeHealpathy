namespace Contract.Requests.Progress.SurveyRequests;

public sealed class DeleteSurveyCommand : DeleteCommand
{
    public DeleteSurveyCommand(Guid id, Guid userId, bool isCompensating = false)
        : base(id, userId, isCompensating)
    {
    }
}