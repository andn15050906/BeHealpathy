using Contract.Domain.Shared.MultimediaBase;
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
    public async Task<IActionResult> Create([FromForm] CreateChatMessageDto dto, [FromServices] IFileService fileService)
    {
        var id = Guid.NewGuid();
        List<Multimedia> medias = [];
        if (dto.Medias is not null)
            medias.AddRange(await fileService.SaveMediasAndUpdateDtos(dto.Medias.Select(_ => (_, id)).ToList()));

        CreateChatMessageCommand command = new(id, dto, ClientId, medias);
        return await Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdateChatMessageDto dto, [FromServices] IFileService fileService)
    {
        List<Multimedia> addedMedias = [];
        var mediaDtos = dto.AddedMedias is null
            ? []
            : dto.AddedMedias.Select(_ => (_, dto.Id)).ToList();
        if (mediaDtos is not null && mediaDtos.Count > 0)
        {
            var medias = await fileService.SaveMediasAndUpdateDtos(mediaDtos!);
            if (medias is not null)
                addedMedias.AddRange(medias!);
        }
        List<Guid> removedMedias = dto.RemovedMedias?.ToList() ?? [];

        UpdateChatMessageCommand command = new(dto, ClientId, addedMedias, removedMedias);
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