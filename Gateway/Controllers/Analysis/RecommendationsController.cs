using Contract.Messaging.ApiClients.Http;
using Gateway.Services.AI.ChatBot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gateway.Controllers.Analysis;

public sealed class RecommendationsController : ContractController
{
    public RecommendationsController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("roadmap")]
    [Authorize]
    public async Task<IActionResult> GetRecommendedRoadmap([FromServices] HealpathyContext context)
    {
        var submissions = await StatisticsController.GetUserSubmissions(context, ClientId, null, null, ["DASS", "First"]);
        var preferences = await StatisticsController.GetUserPreferences(context, ClientId);

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

        return Ok(strictRoadmap?.Id);
    }













    // Answer Questions of the day
    // Track Mood
    // Practice Yoga
    // Enroll in Courses

    /*
General_Activity_Created

Submission_Created
Routine_Created
Routine_Updated
RoutineLog_Created
RoutineLog_Updated
DiaryNote_Created
DiaryNote_Updated
	
Article_Created
ArticleComment_Created
ArticleReaction_Created
MediaResource_Created

Conversation_Created
Conversation_Joined
Conversation_Left
ChatMessage_Created
MessageReaction_Created
Meeting_Created
Meeting_Joined

Advisor_Created
Course_Created
Course_Enrolled
Course_Unenrolled
CourseReview_Created
LectureComment_Created
*/
}