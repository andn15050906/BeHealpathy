using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Progress.RoadmapRequests.Dtos;
using Contract.Requests.Progress.RoadmapRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Progress;

public sealed class RoadmapsController : ContractController
{
    public RoadmapsController(IMediator mediator) : base(mediator)
    {
    }


    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] QueryRoadmapDto dto)
    {
        GetPagedRoadmapsQuery query = new(dto);
        return await Send(query);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CURoadmapDto dto)
    {
        CURoadmapCommand command = new(Guid.NewGuid(), dto, ClientId);
        return await Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update(CURoadmapDto dto)
    {
        if (dto.Id is null)
            return BadRequest();

        CURoadmapCommand command = new((Guid)dto.Id, dto, ClientId);
        return await Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteRoadmapCommand command = new(id, ClientId);
        return await Send(command);
    }
}