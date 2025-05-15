using Contract.Domain.Shared.MultimediaBase;
using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Library.MediaRequests;
using Contract.Requests.Library.MediaRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Tools;

public sealed class MediaResourcesController : ContractController
{
    public MediaResourcesController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPaged([FromQuery] QueryMediaResourceDto dto)
    {
        GetPagedMediaResourcesQuery query = new(dto, ClientId);
        return await Send(query);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreateMediaResourceDto dto, [FromServices] IFileService fileService)
    {
        var id = Guid.NewGuid();
        Multimedia media = (await fileService.SaveMediasAndUpdateDtos([(dto.Media, id)])).First();

        CreateMediaResourceCommand command = new(id, dto, ClientId, media);
        return await Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdateMediaResourceDto dto, [FromServices] IFileService fileService)
    {
        Multimedia? media = null;

        if (dto.ReplacedMedia != null &&
            (dto.ReplacedMedia.File != null || !string.IsNullOrWhiteSpace(dto.ReplacedMedia.Url)))
        {
            var mediaList = await fileService.SaveMediasAndUpdateDtos([(dto.ReplacedMedia, dto.Id)]);
            media = mediaList.FirstOrDefault();
            if (media == null)
                return StatusCode(500, "Media processing failed.");
        }

        var command = new UpdateMediaResourceCommand(dto, ClientId, media);
        return await Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteMediaResourceCommand command = new(id, ClientId);
        return await Send(command);
    }
}