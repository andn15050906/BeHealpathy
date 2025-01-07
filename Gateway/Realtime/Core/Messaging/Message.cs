namespace Gateway.Realtime.Core.Messaging;

public sealed class Message
{
    public Guid? SenderId { get; set; }
    public Guid? ReceiverId { get; set; }
    public MessageTypes Type { get; set; }
    public string Callback { get; set; } = null!;
    public string Data { get; set; } = null!;
    public DateTime Time { get; set; }
}
