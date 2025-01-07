namespace Contract.Requests.Library.MediaRequests;

public sealed class UpdateMediaResourceCommand : UpdateCommand
{
    public UpdateMediaResourceCommand(bool isCompensating = false) : base(isCompensating)
    {
    }
}