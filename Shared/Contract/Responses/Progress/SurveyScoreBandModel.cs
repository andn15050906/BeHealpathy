namespace Contract.Responses.Progress;

public sealed class SurveyScoreBandModel
{
    public int MinScore { get; set; }
    public int MaxScore { get; set; }
    public string BandName { get; set; }
    public string BandRating { get; set; }
}