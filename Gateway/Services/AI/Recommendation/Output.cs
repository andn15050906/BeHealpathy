using MLService.DataStructures;

namespace Gateway.Services.AI.Recommendation;

public class Output
{
    public class Analysis
    {
        public SentimentPrediction Prediction { get; set; }

        public List<string> Emotions { get; set; } = [];
        public List<string> Keywords { get; set; } = [];
        public List<string> Topics { get; set; } = [];
    }
}
