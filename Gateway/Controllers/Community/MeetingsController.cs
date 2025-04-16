using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Community.MeetingRequests;
using Contract.Requests.Community.MeetingRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Community;

public class MeetingsController : ContractController
{
    public MeetingsController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPaged([FromQuery] QueryMeetingDto dto)
    {
        GetPagedMeetingsQuery query = new(dto, ClientId);
        return await Send(query);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateMeetingDto dto)
    {
        CreateMeetingCommand command = new(Guid.NewGuid(), dto, ClientId);
        return await Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update(UpdateMeetingDto dto)
    {
        UpdateMeetingCommand command = new(dto, ClientId);
        return await Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteMeetingCommand command = new(id, ClientId);
        return await Send(command);
    }
}