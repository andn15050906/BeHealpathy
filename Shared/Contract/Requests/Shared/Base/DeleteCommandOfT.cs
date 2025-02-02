namespace Contract.Requests.Shared.Base;

public abstract class DeleteCommand<T> : IRequest<Result<T>>
{
    public Guid Id { get; init; }
    public Guid UserId { get; set; }
    public bool IsCompensating { get; set; }

    public DeleteCommand(Guid id, Guid userId, bool isCompensating = false)
    {
        Id = id;
        UserId = userId;
        IsCompensating = isCompensating;
    }
}