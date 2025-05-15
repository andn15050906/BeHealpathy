using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Progress.RoutineRequests.Dtos;
using Contract.Requests.Progress.RoutineRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Tools;

public sealed class RoutinesController : ContractController
{
    public RoutinesController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPaged([FromQuery] QueryRoutineDto dto)
    {
        GetPagedRoutinesQuery query = new(dto, ClientId);
        return await Send(query);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateRoutineDto dto)
    {
        CreateRoutineCommand command = new(Guid.NewGuid(), dto, ClientId);
        return await Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update(UpdateRoutineDto dto)
    {
        UpdateRoutineCommand command = new(dto, ClientId);
        return await Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteRoutineCommand command = new(id, ClientId);
        return await Send(command);
    }
}