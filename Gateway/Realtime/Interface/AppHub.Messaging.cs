using System.Text.Json;
using Contract.BusinessRules;
using Contract.Helpers;
using Contract.Requests.Community.ChatMessageRequests;
using Contract.Requests.Community.ChatMessageRequests.Dtos;
using Contract.Requests.Community.MessageReactionRequests;
using Contract.Requests.Shared.BaseDtos.Reactions;
using Gateway.Realtime.Core.Messaging;
using Gateway.Services.AI.ChatBot;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace WisNet.Gateway.Realtime.Interface;

public sealed partial class AppHub
{
    // files -> fileNames
    // Moving mediator to another method (not awaited) might cause Connection closing / disposed ???

    public async Task CreateChatMessage(Message message, [FromServices] IMediator mediator, [FromServices] IAppLogger logger)
    {
        Guid clientId = GetClientId();
        if (clientId == default)
            return;
        var dto = JsonSerializer.Deserialize<CreateChatMessageDto>(message.Dto);
        if (dto is null)
            return;

        CreateChatMessageCommand command = new(Guid.NewGuid(), dto, clientId,/**/ []);
        var createMessageResult = await mediator.Send(command);
        if (!createMessageResult.IsSuccessful)
            return;
        var memberIds = message.ConversationMembers.Select(_ => _.ToString());
        try
        {
            await Clients.Users(memberIds).SendAsync(message.Callback, createMessageResult.Data);
        }
        catch (Exception ex)
        {
            logger.Warn(ex.Message);
        }

        try
        {
            if (memberIds.Contains(PreSet.SystemUserId.ToString()))
            {
                Result<string> reply = new();
                try
                {
                    reply = await GeminiClient.Instance.Prompt(dto.Content);
                }
                catch (Exception ex)
                {
                    reply = new Result<string>(200) { Data = ex.Message };
                }

                CreateChatMessageCommand replyCommand = new(Guid.NewGuid(), new CreateChatMessageDto
                {
                    Content = reply.Data ?? string.Empty,
                    ConversationId = dto.ConversationId,            // also ClientId
                }, PreSet.SystemUserId, []);
                var replyMessage = await mediator.Send(replyCommand);
                await Clients.User(clientId.ToString()).SendAsync(message.Callback, replyMessage.Data);
            }
        }
        catch (Exception ex)
        {
            logger.Warn(ex.Message);
        }
    }

    public async Task UpdateChatMessage(Message message, [FromServices] IMediator mediator)
    {
        Guid clientId = GetClientId();
        if (clientId == default)
            return;
        var dto = JsonSerializer.Deserialize<UpdateChatMessageDto>(message.Dto);
        if (dto is null)
            return;

        UpdateChatMessageCommand command = new(dto, clientId,/**/ [],/**/ []);
        var updateMessageResult = await mediator.Send(command);
        if (!updateMessageResult.IsSuccessful)
            return;
        var memberIds = message.ConversationMembers.Select(_ => _.ToString());
        await Clients.Users(memberIds).SendAsync(message.Callback, updateMessageResult.Data);
    }

    public async Task DeleteChatMessage(Message message, [FromServices] IMediator mediator)
    {
        Guid clientId = GetClientId();
        if (clientId == default)
            return;
        bool isValidGuid = Guid.TryParse(message.Dto, out Guid id);
        if (!isValidGuid)
            return;

        DeleteChatMessageCommand command = new(id, clientId);
        var deleteMessageResult = await mediator.Send(command);
        if (!deleteMessageResult.IsSuccessful)
            return;
        var memberIds = message.ConversationMembers.Select(_ => _.ToString());
        await Clients.Users(memberIds).SendAsync(message.Callback, deleteMessageResult.Data);
    }






    public async Task CreateMessageReaction(Message message, [FromServices] IMediator mediator)
    {
        Guid clientId = GetClientId();
        if (clientId == default)
            return;
        var dto = JsonSerializer.Deserialize<CreateReactionDto>(message.Dto);
        if (dto is null)
            return;

        CreateMessageReactionCommand command = new(Guid.NewGuid(), dto, clientId);
        var createReactionResult = await mediator.Send(command);
        if (!createReactionResult.IsSuccessful)
            return;
        var memberIds = message.ConversationMembers.Select(_ => _.ToString());
        await Clients.Users(memberIds).SendAsync(message.Callback, createReactionResult.Data);
    }

    public async Task DeleteMessageReaction(Message message, [FromServices] IMediator mediator)
    {
        Guid clientId = GetClientId();
        if (clientId == default)
            return;
        bool isValidGuid = Guid.TryParse(message.Dto, out Guid id);
        if (!isValidGuid)
            return;

        DeleteMessageReactionCommand command = new(id, clientId);
        var deleteReactionResult = await mediator.Send(command);
        if (!deleteReactionResult.IsSuccessful)
            return;
        var memberIds = message.ConversationMembers.Select(_ => _.ToString());
        await Clients.Users(memberIds).SendAsync(message.Callback, deleteReactionResult.Data);
    }
}
