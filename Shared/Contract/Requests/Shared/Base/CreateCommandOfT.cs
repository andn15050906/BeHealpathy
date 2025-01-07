using Contract.Messaging.Models;

namespace Contract.Requests.Shared.Base;

public abstract class CreateCommand<T> : IRequest<Result<T>>
{
    public Guid Id { get; init; }
    public bool IsCompensating { get; set; }

    public CreateCommand(Guid id, bool isCompensating)
    {
        Id = id;
        IsCompensating = isCompensating;
    }
}
