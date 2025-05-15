using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Shared;

[ApiController]
[Route("api/[controller]")]
public sealed class HealthCheckController : ControllerBase
{
    [HttpGet]
    public void Check()
    {
    }

    [HttpGet("switch-payment")]
    public void SwitchPayment()
    {

    }
}