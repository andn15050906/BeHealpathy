using Contract.Requests.Statistics;

namespace Contract.Services.Contracts;

public interface ICalculationApiService
{
    Task<Result<List<AnalysisOutputByDay>>> PredictSentiment(GetSentimentPredictionQuery query);

    Task<Result<List<string>>> GetRecommendationCourses(GetCourseRecommendationQuery query);
    Task<Result<List<string>>> GetRecommendationArticles(GetArticleRecommendationQuery command);
}