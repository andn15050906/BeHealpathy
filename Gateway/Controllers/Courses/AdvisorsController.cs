using Contract.Domain.Shared.MultimediaBase;
using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Courses.AdvisorRequests;
using Contract.Requests.Courses.AdvisorRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Community;

public sealed class AdvisorsController : ContractController
{
    public AdvisorsController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPaged([FromQuery] QueryAdvisorDto dto)
    {
        GetPagedAdvisorsQuery query = new(dto);
        return await Send(query);
    }

    //[HttpPost]
    //[Authorize]
    //public async Task<IActionResult> Create([FromForm] CreateAdvisorDto dto, [FromServices] IFileService fileService)
    //{
    //    var id = Guid.NewGuid();
    //    List<Multimedia> medias = [];
    //    if (dto.Qualifications is not null)
    //        medias.AddRange(await fileService.SaveMediasAndUpdateDtos(dto.Qualifications.Select(_ => (_, id)).ToList()));

    //    CreateAdvisorCommand command = new(id, dto, ClientId, medias);
    //    return await Send(command);
    //}

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdateAdvisorDto dto, [FromServices] IFileService fileService)
    {
        List<Multimedia> addedMedias = [];
        var mediaDtos = dto.AddedQualifications is null
            ? []
            : dto.AddedQualifications.Select(_ => (_, dto.Id)).ToList();
        if (mediaDtos is not null && mediaDtos.Count > 0)
        {
            var medias = await fileService.SaveMediasAndUpdateDtos(mediaDtos!);
            if (medias is not null)
                addedMedias.AddRange(medias!);
        }
        List<Guid> removedMedias = dto.RemovedQualifications?.ToList() ?? [];

        UpdateAdvisorCommand command = new(dto, ClientId, addedMedias, removedMedias);
        return await Send(command);
    }
}