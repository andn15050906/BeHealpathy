namespace Contract.Requests.Identity.PreferenceRequests.Dtos;

public sealed class UpdatePreferenceDto
{
    public Guid SourceId { get; set; }
    public List<Guid> PreferenceValueIds { get; set; }
}