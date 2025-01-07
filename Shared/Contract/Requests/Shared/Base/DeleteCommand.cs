namespace Contract.Requests.Shared.Base;

public abstract class DeleteCommand : IRequest<Result>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public bool IsCompensating { get; set; }

    public DeleteCommand(Guid id, Guid userId, bool isCompensating = false)
    {
        Id = id;
        UserId = userId;
        IsCompensating = isCompensating;
    }
}
