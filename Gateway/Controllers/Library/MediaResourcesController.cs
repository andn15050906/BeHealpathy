using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Library.MediaRequests;
using Contract.Requests.Library.MediaRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Library;

public sealed class MediaResourcesController : ContractController
{
    public MediaResourcesController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPaged([FromQuery] QueryMediaResourceDto dto)
    {
        GetPagedMediaResourcesQuery query = new(dto, ClientId);
        return await Send(query);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateMediaResourceDto dto)
    {
        CreateMediaResourceCommand command = new(Guid.NewGuid(), dto, ClientId);
        return await Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteMediaResourceCommand command = new(id, ClientId);
        return await Send(command);
    }
}