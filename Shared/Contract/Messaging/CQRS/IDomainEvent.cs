namespace Contract.Messaging.CQRS;

public interface IDomainEvent : INotification
{
    public Guid Id { get; init; }
}
