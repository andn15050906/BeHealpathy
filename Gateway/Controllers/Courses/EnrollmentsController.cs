using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Courses.EnrollmentRequests;
using Contract.Requests.Courses.EnrollmentRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Controllers.Courses;

public sealed class EnrollmentsController : ContractController
{
    public EnrollmentsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get([FromQuery] QueryEnrollmentDto dto)
    {
        GetPagedEnrollmentsQuery query = new(dto, ClientId);
        return await Send(query);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreateEnrollmentDto dto)
    {
        CreateEnrollmentCommand command = new(Guid.NewGuid(), dto, ClientId);
        return await Send(command);
    }

    [HttpPost("update-progress/{courseId}/{index}")]
    [Authorize]
    public async Task<IActionResult> UpdateProgress(Guid courseId, int index, [FromServices] HealpathyContext context)
    {
        try
        {
            var progress = await context.CourseProgress.FirstOrDefaultAsync(_ => _.CourseId == courseId && _.CreatorId == ClientId && !_.IsDeleted);
            if (progress is not null)
            {
                progress.CurrentIndex = index;
            }

            await context.SaveChangesAsync();
            return Ok(progress);
        }
        catch (Exception ex)
        {
            return new JsonResult(ex.Message) { StatusCode = 500 };
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteEnrollmentCommand command = new(id, ClientId);
        return await Send(command);
    }
}