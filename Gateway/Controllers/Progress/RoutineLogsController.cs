using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Progress.RoutineLogRequests;
using Contract.Requests.Progress.RoutineLogRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Library;

public sealed class RoutineLogssController : ContractController
{
    public RoutineLogssController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPaged([FromQuery] QueryRoutineLogDto dto)
    {
        GetPagedRoutineLogsQuery query = new(dto, ClientId);
        return await Send(query);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateRoutineLogDto dto)
    {
        CreateRoutineLogCommand command = new(Guid.NewGuid(), dto, ClientId);
        return await Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdateRoutineLogDto dto)
    {
        UpdateRoutineLogCommand command = new(dto, ClientId);
        return await Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteRoutineLogCommand command = new(id, ClientId);
        return await Send(command);
    }
}