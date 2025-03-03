using Microsoft.AspNetCore.Mvc;

namespace Contract.Messaging.ApiClients.Http;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    // throw Exception
    protected Guid ClientId => (Guid)HttpContext.GetClientId()!;

    // throw Exception
    protected Guid? AdvisorId => HttpContext.GetAdvisorId();
}
