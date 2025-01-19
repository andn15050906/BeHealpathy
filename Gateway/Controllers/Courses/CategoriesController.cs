using Contract.Domain.UserAggregate.Constants;
using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Courses.CategoryRequests;
using Contract.Requests.Courses.CategoryRequests.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Courses;

public sealed class CategoriesController : ContractController
{
    public CategoriesController(IMediator mediator) : base(mediator)
    {
    }



    [HttpGet]
    [ResponseCache(Duration = 60)]
    public async Task<IActionResult> GetAll()
    {
        GetAllCategoriesQuery query = new();
        return await Send(query);
    }

    [HttpPost]
    [Authorize(Roles = RoleConstants.ADMIN)]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        var id = Guid.NewGuid();
        CreateCategoryCommand command = new(id, dto, ClientId);
        return await Send(command);
    }

    [HttpPatch]
    [Authorize(Roles = RoleConstants.ADMIN)]
    public async Task<IActionResult> Update(UpdateCategoryDto dto)
    {
        UpdateCategoryCommand command = new(dto, ClientId);
        return await Send(command);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = RoleConstants.ADMIN)]
    public async Task<IActionResult> Delete(Guid id)
    {
        DeleteCategoryCommand command = new(id, ClientId);
        return await Send(command);
    }
}