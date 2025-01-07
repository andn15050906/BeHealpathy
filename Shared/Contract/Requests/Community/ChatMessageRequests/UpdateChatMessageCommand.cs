namespace Contract.Requests.Community.ChatMessageRequests;

public sealed class UpdateChatMessageCommand : UpdateCommand
{
    public UpdateChatMessageCommand(bool isCompensating = false) : base(isCompensating)
    {
    }
}
