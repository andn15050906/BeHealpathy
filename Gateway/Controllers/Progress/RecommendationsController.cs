using Contract.Domain.ProgressAggregate;
using Contract.Messaging.ApiClients.Http;
using Gateway.Services.AI.ChatBot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Gateway.Controllers.Progress.RoadmapsController;

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
        var quotes = new List<Recommendation>()
        {
            new Recommendation
            {
                Content = "Hãy dành 5 phút mỗi ngày để thực hành hít thở sâu. Điều này sẽ giúp giảm căng thẳng và cải thiện tập trung.",
                IsMotivation = false,
                Source = "TS. Nguyễn An Tâm, Chuyên gia tâm lý"
            },
            new Recommendation
            {
                Content = "Mỗi ngày hãy ghi lại 3 điều bạn biết ơn. Thói quen này sẽ giúp bạn tập trung vào những điều tích cực trong cuộc sống.",
                IsMotivation = false,
                Source = "Nghiên cứu về Tâm lý học Tích cực"
            },
            new Recommendation
            {
                Content = "Hãy nhớ rằng, mỗi bước nhỏ đều quan trọng. Bạn không cần phải hoàn hảo, chỉ cần tiến bộ mỗi ngày.",
                IsMotivation = true,
                Source = ""
            },
            new Recommendation
            {
                Content = "Thử thách không phải để đánh bại bạn, mà để giúp bạn khám phá sức mạnh bên trong mình.",
                IsMotivation = true,
                Source = ""
            }
        };

        return Ok(quotes);
    }

    [HttpGet("recommendations")]
    public IActionResult GetRecommendationData()
    {
        return Ok(DataService.GetRecommendationData());
    }

    [HttpGet("completion")]
    public IActionResult GetCompletionData()
    {
        return Ok(DataService.GetCompletionData());
    }

    [HttpGet("completion-view")]
    public IActionResult GetCompletionViewData()
    {
        return Ok(DataService.GetCompletionViewData());
    }
}