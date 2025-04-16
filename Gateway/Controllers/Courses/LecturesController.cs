using Contract.Domain.Shared.MultimediaBase;
using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Courses.LectureRequests;
using Contract.Requests.Courses.LectureRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Courses;

public sealed class LecturesController : ContractController
{
    public LecturesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] QueryLectureDto dto)
    {
        var clientId = HttpContext.GetClientId();
        GetPagedLecturesQuery query = new(dto, clientId);
        return await Send(query);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreateLectureDto dto, [FromServices] IFileService fileService)
    {
        if (AdvisorId is null)
            return Forbid();

        var id = Guid.NewGuid();

        List<Multimedia> medias = [];
        if (dto.Medias is not null)
            medias.AddRange(await fileService.SaveMediasAndUpdateDtos(dto.Medias.Select(_ => (_, id)).ToList()));

        CreateLectureCommand command = new(id, dto, ClientId, (Guid)AdvisorId, medias);
        return await Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdateLectureDto dto, [FromServices] IFileService fileService)
    {
        if (AdvisorId is null)
            return Forbid();

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

        UpdateLectureCommand command = new(dto, ClientId, (Guid)AdvisorId, addedMedias, removedMedias);
        return await Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteLectureCommand command = new(id, ClientId);
        return await Send(command);
    }
}