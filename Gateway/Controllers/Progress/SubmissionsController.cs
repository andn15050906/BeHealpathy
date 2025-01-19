using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Progress.SubmissionRequests;
using Contract.Requests.Progress.SubmissionRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Progress;


public sealed class SubmissionsController : ContractController
{
    public SubmissionsController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPaged([FromQuery] QuerySubmissionDto dto)
    {
        GetPagedSubmissionsQuery query = new(dto, ClientId);
        return await Send(query);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateSubmissionDto dto)
    {
        CreateSubmissionCommand command = new(Guid.NewGuid(), dto, ClientId);
        return await Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteSubmissionCommand command = new(id, ClientId);
        return await Send(command);
    }
}