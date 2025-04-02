using Contract.Requests.Statistics;
using Contract.Responses.Statistics;

namespace Contract.Services.Contracts;

public interface ICalculationApiService
{
    Task<Result<Output.Analysis>> PredictSentiment(GetSentimentPredictionQuery query);

    Task<Result<List<string>>> GetRecommendationCourses(GetCourseRecommendationQuery query);
    Task<Result<List<string>>> GetRecommendationArticles(GetArticleRecommendationQuery command);
}