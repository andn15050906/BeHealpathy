using Contract.Domain.Shared.MultimediaBase;
using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Progress.DiaryNoteRequests;
using Contract.Requests.Progress.DiaryNoteRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Tools;


public sealed class DiaryNotesController : ContractController
{
    public DiaryNotesController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPaged([FromQuery] QueryDiaryNoteDto dto)
    {
        GetPagedDiaryNotesQuery query = new(dto, ClientId);
        return await Send(query);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreateDiaryNoteDto dto, [FromServices] IFileService fileService)
    {
        var id = Guid.NewGuid();

        List<Multimedia> medias = [];
        if (dto.Medias is not null)
            medias.AddRange(await fileService.SaveMediasAndUpdateDtos(dto.Medias.Select(_ => (_, id)).ToList()));

        CreateDiaryNoteCommand command = new(id, dto, ClientId, medias);
        return await Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdateDiaryNoteDto dto, [FromServices] IFileService fileService)
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

        UpdateDiaryNoteCommand command = new(dto, ClientId, addedMedias, removedMedias);
        return await Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteDiaryNoteCommand command = new(id, ClientId);
        return await Send(command);
    }
}