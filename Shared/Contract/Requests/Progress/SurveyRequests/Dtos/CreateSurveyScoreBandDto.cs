namespace Contract.Requests.Progress.SurveyRequests.Dtos;

public sealed class CreateSurveyScoreBandDto
{
    public int MinScore { get; set; }
    public int MaxScore { get; set; }
    public string BandName { get; set; }
    public string BandRating { get; set; }
}