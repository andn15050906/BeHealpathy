namespace Gateway.Services.AI.Recommendation;

public class Output
{
    public class Analysis
    {
        public string Sentiment { get; set; } = string.Empty;
        public List<string> Emotions { get; set; } = [];
        public List<string> Keywords { get; set; } = [];
        public List<string> Topics { get; set; } = [];
    }
}
