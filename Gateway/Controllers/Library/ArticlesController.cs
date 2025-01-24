using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Community.ArticleRequests;
using Contract.Requests.Library.ArticleRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Contract.Requests.Library.ArticleRequests;
using Contract.Domain.Shared.MultimediaBase;

namespace Gateway.Controllers.Library;

public sealed class ArticlesController : ContractController
{
    public ArticlesController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] QueryArticleDto dto)
    {
        GetPagedArticlesQuery query = new(dto);
        return await Send(query);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreateArticleDto dto, [FromServices] IFileService fileService)
    {
        var id = Guid.NewGuid();
        foreach (var section in dto.Sections)
            section.Id = Guid.NewGuid();

        List<Multimedia> medias = [];
        if (dto.Thumb is not null)
            medias.Add(await fileService.SaveImageAndUpdateDto(dto.Thumb, id));
        var sectionMediaDtos = dto.Sections.Where(_ => _.Media is not null).Select(_ => (_.Media!, _.Id)).ToList();
        if (sectionMediaDtos is not null)
            medias.AddRange(await fileService.SaveMediasAndUpdateDtos(sectionMediaDtos));

        CreateArticleCommand command = new(id, dto, ClientId, medias);
        return await Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdateArticleDto dto, [FromServices] IFileService fileService)
    {
        List<Multimedia> addedMedias = [];
        if (dto.Thumb is not null)
            addedMedias.Add(await fileService.SaveImageAndUpdateDto(dto.Thumb, dto.Id));
        var sectionMediaDtos = dto.Sections is null
            ? []
            : dto.Sections.Where(_ => _.AddedMedia is not null).Select(_ => (_.AddedMedia, _.Id)).ToList();
        if (sectionMediaDtos is not null && sectionMediaDtos.Count > 0)
        {
            var sectionMedias = await fileService.SaveMediasAndUpdateDtos(sectionMediaDtos!);
            if (sectionMedias is not null)
                addedMedias.AddRange(sectionMedias!);
        }
        List<Guid> removedMedias = dto.Sections?.Select(_ => _.RemovedMedia).ToList() ?? [];

        UpdateArticleCommand command = new(dto, ClientId, addedMedias, removedMedias);
        return await Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteArticleCommand command = new(id, ClientId);
        return await Send(command);
    }
}