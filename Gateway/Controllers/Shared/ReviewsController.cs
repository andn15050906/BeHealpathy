using Contract.Domain.Shared.MultimediaBase;
using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Shared.BaseDtos.Reviews;
using Contract.Requests.Shared.BaseRequests.Reviews;
using Contract.Requests.Shared;
using Contract.Responses.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Contract.Requests.Library.CourseReviewRequests;

namespace Gateway.Controllers.Shared;

public class ReviewsController : ContractController
{
    public ReviewsController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] QueryReviewDto dto)
    {
        IRequest<Result<PagedResult<ReviewModel>>> request;

        switch (dto.TargetEntity)
        {
            case TargetEntity.CourseReview:
                request = new GetPagedCourseReviewsQuery(dto);
                return await Send(request);
            default:
                return BadRequest(nameof(TargetEntity));
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreateReviewDto dto, [FromServices] IFileService fileService)
    {
        var id = Guid.NewGuid();

        List<Multimedia> medias = [];
        if (dto.Medias is not null)
            medias.AddRange(await fileService.SaveMediasAndUpdateDtos(dto.Medias.Select(_ => (_, id)).ToList()));

        CreateReviewCommand command = new(id, dto, ClientId, medias);
        return await Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdateReviewDto dto, [FromServices] IFileService fileService)
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

        UpdateReviewCommand command = new(dto, ClientId, addedMedias, removedMedias);
        return await Send(command);
    }
}