using Algolia.Search.Clients;
using Algolia.Search.Models.Recommend;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers.Recommendation
{
    [Route("you-may-like")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly RecommendClient _recommendClient;
        private readonly string _appId = "HSWLKQ4RB4";
        private readonly string _recommendationKey = "935eb67488f0490049bd08c39c2912a9";
        private readonly string _indexName = "courses";

        public RecommendationController()
        {
            _recommendClient = new RecommendClient(_appId, _recommendationKey);
        }

        [HttpGet("courses")]
        public async Task<IActionResult> GetRecommendationCourses()
        {
            var response = await _recommendClient.GetRecommendationsAsync(
                new GetRecommendationsParams
                {
                    Requests = new List<RecommendationsRequest>
                    {
                        new RecommendationsRequest(
                            new TrendingItemsQuery
                            {
                                IndexName = _indexName,
                                Model = TrendingItemsModel.TrendingItems
                            }
                        )
                    }
                }
            );

            var hits = response.Results.SelectMany(r => r.Hits).ToList();
            return Ok(hits);

        }
    }
}
