using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Community.ArticleRequests;
using Contract.Requests.Library.ArticleRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Contract.Requests.Library.ArticleRequests;

namespace Gateway.Controllers.Library;

public sealed class ArticlesController : ContractController
{
    public ArticlesController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPaged([FromQuery] QueryArticleDto dto)
    {
        GetPagedArticlesQuery query = new(dto, ClientId);
        return await Send(query);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateArticleDto dto)
    {
        CreateArticleCommand command = new(Guid.NewGuid(), dto, ClientId);
        return await Send(command);
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdateArticleDto dto)
    {
        UpdateArticleCommand command = new(dto, ClientId);
        return await Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteArticleCommand command = new(id, ClientId);
        return await Send(command);
    }
}