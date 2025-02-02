namespace Contract.Requests.Shared.Base;

public abstract class UpdateCommand<T> : IRequest<Result<T>>
{
    public bool IsCompensating { get; set; }

    public UpdateCommand(bool isCompensating)
    {
        IsCompensating = isCompensating;
    }
}