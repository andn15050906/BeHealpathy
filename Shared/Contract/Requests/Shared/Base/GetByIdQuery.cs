namespace Contract.Requests.Shared.Base;

public abstract class GetByIdQuery<T> : IRequest<Result<T>>
{
    public Guid Id { get; private set; }

    public GetByIdQuery(Guid id)
    {
        Id = id;
    }
}
