namespace Gateway.Realtime.Core.Messaging;

// Should not apply DataOperation, following Interface Segregation Principle
public sealed class Message
{
    // Send the member list instead of making another DB call
    public List<Guid> ConversationMembers { get; set; } = [];

    public string Callback { get; set; } = null!;

    public string Dto { get; set; } = null!;
}