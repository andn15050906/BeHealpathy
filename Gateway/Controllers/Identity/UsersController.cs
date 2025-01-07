using Contract.Helpers.AppExploration;
using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Identity;
using Contract.Requests.Identity.Dtos;
using Infrastructure.Helpers.Email;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Gateway.Controllers.Identity;

public sealed class UsersController : ContractController
{
    public UsersController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        GetUserByIdQuery query = new(id);
        return await Send(query);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] QueryUserDto dto)
    {
        GetPagedUsersQuery query = new(dto);
        return await Send(query);
    }

    [HttpPost]
    public async Task<IActionResult> Insert(
        [FromBody] CreateUserDto dto,
        [FromServices] EmailService emailService, [FromServices] IOptions<AppInfoOptions> appInfo)
    {
        CreateUserCommand command = new(Guid.NewGuid(), dto);
        var result = await _mediator.Send(command);

        if (result.IsSuccessful)
        {
            string link = $"{appInfo.Value.MainFrontendApp}/sign-in?email={dto.Email}&token={result.Data}";
            try
            {
#pragma warning disable CS4014
                await emailService.SendRegistrationEmail(dto.Email, dto.UserName, link);
#pragma warning restore CS4014
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message) { StatusCode = 500 };
            }
        }
        return result.AsResponse();
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateUserDto dto)
    {
        UpdateUserCommand command = new(dto, ClientId);
        return await Send(command);
    }

    //[HttpDelete("{id}")]
    //public async Task<IActionResult> Delete(Guid id)
    //{
    //    await _context.DeleteOne<User>(id);

    //    return Ok(await _context.Find(Builders<User>.Filter.Empty));
    //}

    [HttpPost("verify")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto dto)
    {
        VerifyEmailCommand command = new(dto);
        return await Send(command);
    }
}