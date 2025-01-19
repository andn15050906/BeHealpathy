using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Courses.EnrollmentRequests;
using Contract.Requests.Courses.EnrollmentRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteEnrollmentCommand command = new(id, ClientId);
        return await Send(command);
    }
}