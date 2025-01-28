namespace Contract.Requests.Identity.Dtos;

public sealed class UpdatePreferenceDto
{
    public Guid SourceId { get; set; }
    public List<Guid> PreferenceValueIds { get; set; }
}