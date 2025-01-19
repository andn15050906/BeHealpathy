using Contract.Domain.Shared.MultimediaBase;
using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Courses.LectureCommentRequests;
using Contract.Requests.Library.ArticleCommentRequests;
using Contract.Requests.Shared;
using Contract.Requests.Shared.BaseDtos.Comments;
using Contract.Requests.Shared.BaseRequests.Comments;
using Contract.Responses.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Shared;

public class CommentsController : ContractController
{
    public CommentsController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] QueryCommentDto dto)
    {
        IRequest<Result<PagedResult<CommentModel>>> request;

        switch (dto.TargetEntity)
        {
            case TargetEntity.ArticleComment:
                request = new GetPagedArticleCommentsQuery(dto);
                return await Send(request);
            case TargetEntity.LectureComment:
                request = new GetPagedLectureCommentsQuery(dto);
                return await Send(request);
            default:
                return BadRequest(nameof(TargetEntity));
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreateCommentDto dto, [FromServices] IFileService fileService)
    {
        var id = Guid.NewGuid();

        List<Multimedia> medias = [];
        if (dto.Medias is not null)
            medias.AddRange(await fileService.SaveMediasAndUpdateDtos(dto.Medias.Select(_ => (_, id)).ToList()));

        CreateCommentCommand command = new(id, dto, ClientId, medias);
        return await Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdateCommentDto dto, [FromServices] IFileService fileService)
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

        UpdateCommentCommand command = new(dto, ClientId, addedMedias, removedMedias);
        return await Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteCommentCommand command = new(id, ClientId);
        return await Send(command);
    }
}