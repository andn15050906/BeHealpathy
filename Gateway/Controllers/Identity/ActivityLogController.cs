using System.Text.Json;
using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Identity.ActivityLogRequests;
using Contract.Requests.Identity.ActivityLogRequests.Dtos;
using Contract.Responses.Identity;
using Core.Helpers;
using Gateway.Services.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Identity;

public class ActivityLogController : ContractController
{
    private const string EmptyArray = "[]";
    private string ActivityLogKey => $"ActivityLog_{ClientId}";              //  List<CreateActivityLogDto>


    public ActivityLogController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get([FromQuery] QueryActivityLogDto dto, [FromServices] ICacheService cache)
    {
        var cachedList = (await cache.Get(ActivityLogKey)) ?? EmptyArray;
        var list = JsonSerializer.Deserialize<List<CreateActivityLogDto>>(cachedList) ?? [];
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
            cacheQueryResult.AddRange(dbQueryResult.Data?.Items!);

        return Ok(cacheQueryResult);
    }

    [HttpPost]
    public async Task<IActionResult> Add(List<CreateActivityLogDto> dtos, [FromServices] ICacheService cache)
    {
        // Retrieve cached list
        foreach (var dto in dtos)
            dto.CreationTime = TimeHelper.Now;
        var list = JsonSerializer.Deserialize<List<CreateActivityLogDto>>(
            (await cache.Get(ActivityLogKey)) ?? EmptyArray
        ) ?? [];

        // If Cache is old, Trim the cache and Save to the database
        var firstUpdate = list.FirstOrDefault()?.CreationTime ?? TimeHelper.Now;
        if (firstUpdate.AddHours(1) <= TimeHelper.Now)
        {
            var command = new CreateActivityLogCommand(Guid.NewGuid(), list, ClientId);
            await Task.WhenAll(_mediator.Send(command), cache.Delete(ActivityLogKey));
            list = [];
        }

        list.AddRange(dtos);
        await cache.Set(ActivityLogKey, JsonSerializer.Serialize(list));
        return Ok();
    }
}