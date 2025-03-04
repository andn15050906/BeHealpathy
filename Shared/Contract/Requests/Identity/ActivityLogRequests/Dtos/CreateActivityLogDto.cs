namespace Contract.Requests.Identity.ActivityLogRequests.Dtos;

public sealed class CreateActivityLogDto
{
    public string Content { get; set; } = string.Empty;
    public DateTime? CreationTime { get; set; }                     // No need to add this to the request
}