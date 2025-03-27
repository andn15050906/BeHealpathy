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
    public async Task<IActionResult> GetRecommendedRoadmap([FromServices] HealpathyContext context, [FromServices] IChatbotClient chatbot)
    {
        var submissions = await StatisticsController.GetUserSubmissions(context, ClientId, ["DASS", "First"]);
        var preferences = await StatisticsController.GetUserPreferences(context, ClientId);

        var roadmaps = await context.Roadmaps.ToListAsync();
        var strictRoadmap = roadmaps.FirstOrDefault(_ => _.Title.ToLower().Contains("Strict".ToLower()));
        var lightRoadmap = roadmaps.FirstOrDefault(_ => _.Title.ToLower() == "mental roadmap");

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