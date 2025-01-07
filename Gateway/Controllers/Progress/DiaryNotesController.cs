using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Progress.DiaryNoteRequests;
using Contract.Requests.Progress.DiaryNoteRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Progress;


public sealed class DiaryNotesController : ContractController
{
    public DiaryNotesController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPaged([FromQuery] QueryDiaryNoteDto dto)
    {
        GetPagedDiaryNotesQuery query = new(dto, ClientId);
        return await Send(query);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateDiaryNoteDto dto)
    {
        CreateDiaryNoteCommand command = new(Guid.NewGuid(), dto, ClientId);
        return await Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdateDiaryNoteDto dto)
    {
        UpdateDiaryNoteCommand command = new(dto, ClientId);
        return await Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteDiaryNoteCommand command = new(id, ClientId);
        return await Send(command);
    }
}