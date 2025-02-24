namespace Contract.Domain.ProgressAggregates;

public sealed class SurveyScoreBand : Entity
{
    public int MinScore { get; set; }
    public int MaxScore { get; set; }
    public string BandName { get; set; }
    public string BandRating { get; set; }



    public SurveyScoreBand(int minScore, int maxScore, string bandName, string bandRating)
    {
        MinScore = minScore;
        MaxScore = maxScore;
        BandName = bandName;
        BandRating = bandRating;
    }
}