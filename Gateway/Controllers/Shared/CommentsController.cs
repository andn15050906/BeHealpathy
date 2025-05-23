﻿using Contract.Domain.Shared.MultimediaBase;
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
            case TargetFeedbackEntity.ArticleComment:
                request = new GetPagedArticleCommentsQuery(dto);
                return await Send(request);
            case TargetFeedbackEntity.LectureComment:
                request = new GetPagedLectureCommentsQuery(dto);
                return await Send(request);
            default:
                return BadRequest(BusinessMessages.Comment.INVALID_TARGET_ENTITY);
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreateCommentDto dto, [FromServices] IFileService fileService)
    {
        CreateCommentCommand command;
        var id = Guid.NewGuid();
        switch (dto.TargetEntity)
        {
            case TargetFeedbackEntity.ArticleComment:
                command = new CreateArticleCommentCommand(id, dto, ClientId, null);
                break;
            case TargetFeedbackEntity.LectureComment:
                command = new CreateLectureCommentCommand(id, dto, ClientId, null);
                break;
            default:
                return BadRequest(BusinessMessages.Comment.INVALID_TARGET_ENTITY);
        }

        List<Multimedia> medias = [];
        if (dto.Medias is not null)
            medias.AddRange(await fileService.SaveMediasAndUpdateDtos(dto.Medias.Select(_ => (_, id)).ToList()));
        command.Medias = medias;

        return await Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdateCommentDto dto, [FromServices] IFileService fileService)
    {
        UpdateCommentCommand command;
        switch (dto.TargetEntity)
        {
            case TargetFeedbackEntity.ArticleComment:
                command = new UpdateArticleCommentCommand(dto, ClientId, null, null);
                break;
            case TargetFeedbackEntity.LectureComment:
                command = new UpdateLectureCommentCommand(dto, ClientId, null, null);
                break;
            default:
                return BadRequest(BusinessMessages.Comment.INVALID_TARGET_ENTITY);
        }

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

        command.AddedMedias = addedMedias;
        command.RemovedMedias = removedMedias;

        return await Send(command);
    }

    //[HttpDelete("{id}")]
    //[Authorize]
    //public async Task<IActionResult> Delete(Guid id)
    //{
    //    try
    //    {
    //        return await Send(new DeleteArticleCommentCommand(id, ClientId));
    //    }
    //    catch (Exception)
    //    {
    //        try
    //        {
    //            return await Send(new DeleteLectureCommentCommand(id, ClientId));
    //        }
    //        catch (Exception)
    //        {
    //            return NotFound();
    //        }
    //    }
    //}
    [HttpDelete("article/{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteArticleComment(Guid id)
    {
        return await Send(new DeleteArticleCommentCommand(id, ClientId));
    }

    [HttpDelete("lecture/{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteLectureComment(Guid id)
    {
        return await Send(new DeleteLectureCommentCommand(id, ClientId));
    }
}