using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Community.ChatMessageRequests;
using Contract.Requests.Community.ChatMessageRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Community;

public sealed class ChatMessagesController : ContractController
{
    public ChatMessagesController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPaged([FromQuery] QueryChatMessageDto dto)
    {
        GetPagedChatMessagesQuery query = new(dto, ClientId);
        return await Send(query);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateChatMessageDto dto)
    {
        CreateChatMessageCommand command = new(Guid.NewGuid(), dto, ClientId);
        return await Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteChatMessageCommand command = new(id, ClientId);
        return await Send(command);
    }
}