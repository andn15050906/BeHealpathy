using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Progress.SurveyRequests.Dtos;
using Contract.Requests.Progress.SurveyRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Contract.Helpers.Storage;
using Contract.Helpers;

namespace Gateway.Controllers.Progress;

public sealed class SurveysController : ContractController
{
    private readonly IAppLogger _logger;

    public SurveysController(IMediator mediator, IAppLogger logger) : base(mediator)
    {
        _logger = logger;
    }



    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] QuerySurveyDto dto)
    {
        GetPagedSurveysQuery query = new(dto);
        return await Send(query);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateSurveyDto dto)
    {
        CreateSurveyCommand command = new(Guid.NewGuid(), dto, ClientId);
        return await Send(command);
    }

    [HttpPost("import")]
    [Authorize]
    public async Task<IActionResult> Create(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(BusinessMessages.Survey.INVALID_FILE_FORMAT);

        try
        {
            var dto = FileConverter.ProcessSurveyFromExcelFile(file);
            CreateSurveyCommand command = new(Guid.NewGuid(), dto, ClientId);
            return await Send(command);
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
            return BadRequest(BusinessMessages.Survey.INVALID_FILE_FORMAT);
        }
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update(UpdateSurveyDto dto)
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

    [HttpGet("test")]
    [Authorize]
    public IActionResult TestDeployment()
    {
        return Ok("Test");
    }

    [HttpPost("testPost")]
    [Authorize]
    public IActionResult TestDeploymentPost()
    {
        return Ok("TestPost");
    }
}