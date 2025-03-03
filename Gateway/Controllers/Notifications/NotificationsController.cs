using Contract.Domain.Shared.MultimediaBase;
using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Notifications;
using Contract.Requests.Notifications.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Notifications;

public class NotificationsController : ContractController
{
    public NotificationsController(IMediator mediator) : base(mediator) { }



    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] QueryNotificationDto dto)
    {
        GetPagedNotificationsQuery query = new(dto, ClientId);
        return await Send(query);
    }

    [HttpPost("Advisor")]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreateAdvisorRequestDto dto, [FromServices] IFileService fileService)
    {
        var advisorId = Guid.NewGuid();
        Task<Multimedia?>? cv = dto.CV is not null ? fileService.SaveMediaAndUpdateDto(dto.CV, advisorId) : null;

        List<Multimedia> certificates = [];
        if (dto.Certificates is not null)
            certificates.AddRange(await fileService.SaveMediasAndUpdateDtos(dto.Certificates.Select(_ => (_, advisorId)).ToList()));

        var command = new CreateNotificationCommand(Guid.NewGuid(), advisorId, dto, ClientId, cv is not null ? await cv : null, certificates);
        return await Send(command);
    }

    [HttpPost("Withdrawal")]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreateWithdrawalRequestDto dto)
    {
        CreateNotificationCommand command = new(Guid.NewGuid(), dto, ClientId);
        return await Send(command);
    }

    //[HttpPost("MultiReceiver")]

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update(UpdateNotificationDto dto)
    {
        UpdateNotificationCommand command = new(dto, ClientId);
        return await Send(command);
    }
}