namespace Contract.Requests.Progress.RoadmapRequests.Dtos;

public sealed class CURoadmapDto
{
    public Guid? Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string IntroText { get; set; } = string.Empty;



    public List<CURoadmapPhaseDto> Phases { get; set; } = [];
}