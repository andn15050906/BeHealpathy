using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Progress.SurveyRequests.Dtos;
using Contract.Requests.Progress.SurveyRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Progress;

public sealed class SurveysController : ContractController
{
    public SurveysController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPaged([FromQuery] QuerySurveyDto dto)
    {
        GetPagedSurveysQuery query = new(dto, ClientId);
        return await Send(query);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateSurveyDto dto)
    {
        CreateSurveyCommand command = new(Guid.NewGuid(), dto, ClientId);
        return await Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdateSurveyDto dto)
    {
        UpdateSurveyCommand command = new(dto, ClientId);
        return await Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteSurveyCommand command = new(id, ClientId);
        return await Send(command);
    }
}