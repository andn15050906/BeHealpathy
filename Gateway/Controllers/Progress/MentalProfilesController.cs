using Contract.Messaging.ApiClients.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Controllers.Progress;

public class MentalProfilesController : ContractController
{
    public MentalProfilesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("{userId}")]
    [Authorize]
    public async Task<IActionResult> Get(Guid userId, [FromServices] HealpathyContext context)
    {
        /*var result = await context.MentalProfiles
            .Where(_ => _.CreatorId == userId && !_.IsDeleted)
            .ToListAsync();

        return Ok(result);*/
        
        return Ok(new RoadmapsController.MentalProfile
        {
            UserType = "Người đi làm",
            StressSource = "Công việc",
            ImprovementGoal = "Cân bằng công việc - cuộc sống",
            StressLevel = 65,
            DepressionRisk = 40,
            EmotionalIndex =
            [
                new() { Date = DateTime.Parse("2023-05-05"), Value = 65 },
                new() { Date = DateTime.Parse("2023-05-06"), Value = 58 },
                new() { Date = DateTime.Parse("2023-05-07"), Value = 72 },
                new() { Date = DateTime.Parse("2023-05-08"), Value = 68 },
                new() { Date = DateTime.Parse("2023-05-09"), Value = 75 }
            ]
        });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateUpdate(Contract.Domain.ProgressAggregate.MentalProfile dto, [FromServices] HealpathyContext context)
    {
        var result = await context.MentalProfiles
            .Where(_ => _.Id == dto.Id && _.CreatorId == ClientId && !_.IsDeleted)
            .FirstOrDefaultAsync();

        if (result is null)
        {
            await context.MentalProfiles.AddAsync(dto);
        }
        else
        {
            result.Attribute = dto.Attribute;
            result.Value = dto.Value;
        }

        await context.SaveChangesAsync();
        return Ok(result);
    }
}