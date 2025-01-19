namespace Contract.Requests.Shared.Base;

public abstract class UpdateCommand : IRequest<Result>
{
    public bool IsCompensating { get; set; }

    public UpdateCommand(bool isCompensating)
    {
        IsCompensating = isCompensating;
    }
}
