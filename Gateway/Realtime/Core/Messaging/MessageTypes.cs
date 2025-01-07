namespace Gateway.Realtime.Core.Messaging;

public enum MessageTypes : byte
{
    // Send
    CreateConversation,
    UpdateConversation,

    CreateTextChatMessage,
    CreateFileChatMessage,



    // Receive
    ChatMessageCreated
}