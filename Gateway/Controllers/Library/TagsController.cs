using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Library.TagRequests;
using Contract.Requests.Library.TagRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Library;

public class TagsController : ContractController
{
    public TagsController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    public async Task<IActionResult> GetPaged()
    {
        GetAllTagsQuery query = new();
        return await Send(query);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateTagDto dto)
    {
        CreateTagCommand command = new(Guid.NewGuid(), dto);
        return await Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteTagCommand command = new(id, ClientId);
        return await Send(command);
    }
}