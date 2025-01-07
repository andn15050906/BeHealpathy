using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Community.ConversationRequests;
using Contract.Requests.Community.ConversationRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Community;

public sealed class ConversationsController : ContractController
{
    public ConversationsController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPaged([FromQuery] QueryConversationDto dto)
    {
        GetPagedConversationsQuery query = new(dto, ClientId);
        return await Send(query);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreateConversationDto dto)
    {
        CreateConversationCommand command = new(Guid.NewGuid(), dto, ClientId);
        return await Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdateConversationDto dto)
    {
        UpdateConversationCommand command = new(dto, ClientId);
        return await Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteConversationCommand command = new(id, ClientId);
        return await Send(command);
    }
}
