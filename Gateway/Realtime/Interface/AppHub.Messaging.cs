using System.Text.Json;
using Contract.Requests.Community.ChatMessageRequests;
using Contract.Requests.Community.ChatMessageRequests.Dtos;
using Gateway.Realtime.Core.Messaging;

namespace WisNet.Gateway.Realtime.Interface;

public sealed partial class AppHub
{
    // files -> fileNames
    /*public async Task SendToConversation(Message message)
    {
        Guid clientId = GetClientId();
        if (clientId == default)
            return;
        Guid conversationId = (Guid)message.ReceiverId!;



        CreateChatMessageDto dto = new()
        {
            ConversationId = conversationId
        };
        CreateChatMessageCommand command = new(Guid.NewGuid(), dto, GetClientId());
        if (message.Type == MessageTypes.CreateTextChatMessage)
            dto.Content = message.Data;
        else if (message.Type == MessageTypes.CreateFileChatMessage)
            dto.Attachments = JsonSerializer.Deserialize<List<string>>(message.Data);
        else
            return;



        var conversation = await _conversationService.Get(conversationId, clientId);
        if (conversation is null)
            return;
        var memberIds = conversation.Data!.Members.Select(_ => _.CreatorId.ToString());



        var result = await _chatMessageService.Create(dto, clientId);
        if (result.IsSuccessful)
        {
#pragma warning disable CS4014
            Clients.Users(memberIds).SendAsync(message.Callback, conversationId, result.Data);
#pragma warning restore CS4014
        }
    }*/
}
