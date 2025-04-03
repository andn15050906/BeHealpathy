namespace Contract.Responses.Statistics;

public class Output
{
    public class Analysis
    {
        public bool Prediction { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }

        public List<string> Emotions { get; set; } = [];
        public List<string> Keywords { get; set; } = [];
        public List<string> Topics { get; set; } = [];
    }
}