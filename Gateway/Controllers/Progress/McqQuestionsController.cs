using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Progress;
using Contract.Requests.Progress.McqRequests;
using Contract.Requests.Progress.McqRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Progress;


public sealed class McqQuestionsController : ContractController
{
    public McqQuestionsController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPaged([FromQuery] QueryMcqQuestionDto dto)
    {
        GetPagedMcqQuestionsQuery query = new(dto, ClientId);
        return await Send(query);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateMcqQuestionDto dto)
    {
        CreateMcqQuestionCommand command = new(Guid.NewGuid(), dto, ClientId);
        return await Send(command);
    }

    /*[HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdateMcqQuestionDto dto)
    {
        UpdateMcqQuestionCommand command = new(dto, ClientId);
        return await Send(command);
    }*/

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteMcqQuestionCommand command = new(id, ClientId);
        return await Send(command);
    }
}