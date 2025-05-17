using Contract.Domain.ProgressAggregate;
using Contract.Messaging.ApiClients.Http;
using Gateway.Services.AI.ChatBot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Controllers.Progress;

public sealed class RecommendationsController : ContractController
{
    public RecommendationsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("roadmap")]
    [Authorize]
    public async Task<IActionResult> GetRecommendedRoadmap([FromServices] HealpathyContext context)
    {
        /*
    calculateStressLevel() {
      let level = 20;
      if (this.answers.issue === 'study_pressure' || this.answers.issue === 'work_stress') level += 40;
      if (this.answers.issue === 'bullying' || this.answers.issue === 'intern_stress') level += 30;
      if (this.answers.issue === 'parent_conflict' || this.answers.issue === 'colleague_conflict') level += 20;
      // ... các trường hợp khác
      return Math.min(level, 100);
    },
    calculateDepressionRisk() {
      let risk = 10;
      if (this.answers.issue === 'loneliness' || this.answers.issue === 'no_close_friend') risk += 40;
      if (this.answers.issue === 'no_motivation' || this.answers.issue === 'no_passion') risk += 30;
      if (this.answers.related === 'myself') risk += 20;
      // ... các trường hợp khác
      return Math.min(risk, 100);

         */



        /*var submissions = await StatisticsController.GetUserSubmissions(context, ClientId, null, null, ["DASS", "First"]);

        var roadmaps = await context.Roadmaps.ToListAsync();
        var strictRoadmap = roadmaps.FirstOrDefault(_ => _.Id == new Guid("74224472-5d06-4e89-b10c-d860b0a0f68c"));
        var lightRoadmap = roadmaps.FirstOrDefault(_ => _.Id == new Guid("8cacd52d-bad9-4dbb-b361-f388fd3d46de"));

        var dassEvel = submissions.FirstOrDefault(_ => _.SurveyName.Contains("DASS"));
        if (dassEvel is not null)
        {
            if (dassEvel.SurveyBandInputs.Any(_ => _.Rating.Contains("Severe") || _.Rating.Contains("Bad")))
                return Ok(strictRoadmap?.Id);
        }

        var firstEval = submissions.FirstOrDefault(_ => _.SurveyName.Contains("First"));
        if (firstEval is not null)
        {
            var firstEvalStrictRecommended = firstEval?.Score > 10;
            var recommended = firstEvalStrictRecommended ? strictRoadmap : lightRoadmap;
            return Ok(recommended?.Id ?? roadmaps.First().Id);
        }

        return Ok(strictRoadmap?.Id);*/
        return Ok();
    }

    [HttpGet("quotes")]
    public IActionResult GetQuotes()
    {
        var mentalProfile = new MentalProfile(Guid.NewGuid(), Guid.NewGuid(), "", "");

        return Ok();
    }

    [HttpGet("actions")]
    public IActionResult GetActions()
    {
        return Ok();
    }
}