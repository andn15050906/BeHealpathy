using Contract.IntegrationEvents;
using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Identity.ActivityLogRequests;
using Contract.Requests.Identity.ActivityLogRequests.Dtos;
using Contract.Responses.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Identity;

public class ActivityLogsController : ContractController
{
    public ActivityLogsController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get([FromQuery] QueryActivityLogDto dto, [FromServices] IEventCache cache)
    {
        var list = await cache.GetList<Events.General_Activity_Created>(ClientId) ?? [];
        var cacheQueryResult = list.Select(_ => new ActivityLogModel {
            Id = Guid.Empty,
            Content = _.Content,
            CreationTime = (DateTime)_.CreationTime!,
            CreatorId = ClientId
        }).ToList();

        var query = new GetPagedActivityLogsQuery(dto, ClientId);
        var dbQueryResult = await _mediator.Send(query);

        //...
        if (dbQueryResult.IsSuccessful)
        {
            cacheQueryResult ??= [];
            cacheQueryResult.AddRange(dbQueryResult.Data?.Items!);
        }

        return Ok(cacheQueryResult);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Add(List<CreateActivityLogDto> dtos, [FromServices] IEventCache cache)
    {
        foreach (var dto in dtos)
        {
            cache.Add(ClientId, new Events.General_Activity_Created(dto.Content));
        }
        return new Result(200).AsResponse();
    }
}