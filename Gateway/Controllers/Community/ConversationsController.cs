using Contract.Domain.Shared.MultimediaBase;
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
    public async Task<IActionResult> Create([FromForm] CreateConversationDto dto, [FromServices] IFileService fileService)
    {
        var id = Guid.NewGuid();
        var thumb = await fileService.SaveImageAndUpdateDto(dto.Thumb, id);

        CreateConversationCommand command = new(id, dto, ClientId, thumb);
        return await Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdateConversationDto dto, [FromServices] IFileService fileService)
    {
        Multimedia? thumb = null;
        if (dto.Thumb is not null)
            thumb = await fileService.SaveImageAndUpdateDto(dto.Thumb, dto.Id);

        UpdateConversationCommand command = new(dto, ClientId, thumb);
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
