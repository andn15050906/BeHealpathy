namespace Contract.IntegrationEvents;

public abstract record BaseIntegrationEvent
{
    public override string ToString() => GetType().Name;
}