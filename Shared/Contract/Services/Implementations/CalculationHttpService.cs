using Contract.Messaging.ApiClients.Http;
using Contract.Requests.Statistics;
using Contract.Responses.Statistics;
using Contract.Services.Contracts;

namespace Contract.Services.Implementations;

public sealed class CalculationHttpService(HttpClient httpClient) : HttpApiClient(httpClient), ICalculationApiService
{
    public async Task<Result<List<string>>> GetRecommendationArticles(GetArticleRecommendationQuery command)
        => await GetAsync<List<string>>(API.Calculation.GetRecommendationArticles());

    public async Task<Result<List<string>>> GetRecommendationCourses(GetCourseRecommendationQuery query)
        => await GetAsync<List<string>>(API.Calculation.GetRecommendationCourses());

    public async Task<Result<Output.Analysis>> PredictSentiment(GetSentimentPredictionQuery query)
        => await GetAsync<Output.Analysis>(API.Calculation.PredictUri(query));
}