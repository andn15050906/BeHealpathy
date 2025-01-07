namespace Gateway.Realtime.Core.Streaming.Dtos;

public sealed class OfferMessage
{
    public string Offer { get; set; } = string.Empty;
    public string Peer { get; set; } = string.Empty;
}
