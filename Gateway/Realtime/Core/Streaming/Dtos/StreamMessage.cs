namespace Gateway.Realtime.Core.Streaming.Dtos;

public sealed class StreamMessage
{
    public string Event { get; set; }               // StreamEvents
    public string? Data { get; set; }
}
