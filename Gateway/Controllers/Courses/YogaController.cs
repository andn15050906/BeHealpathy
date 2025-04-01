using Contract.Messaging.ApiClients.Http;
using Microsoft.AspNetCore.Mvc;
using Contract.Requests.Courses.CourseRequests.Dtos;
using Contract.Requests.Courses.CourseRequests;
using Microsoft.AspNetCore.Authorization;

namespace Gateway.Controllers.Courses
{
    [ApiController]
    public sealed class YogaController : ContractController
    {
        public YogaController(IMediator mediator) : base(mediator)
        {
        }

        [Authorize]
        [HttpGet("Poses")]
        public async Task<IActionResult> Get([FromQuery] QueryYogaPoseDto dto)
        {
            GetPagedYogaPoseQuery query = new(dto, ClientId);
            return await Send(query);
        }
    }
}
