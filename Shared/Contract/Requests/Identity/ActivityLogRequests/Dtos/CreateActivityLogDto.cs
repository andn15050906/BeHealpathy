namespace Contract.Requests.Identity.ActivityLogRequests.Dtos;

public sealed class CreateActivityLogDto
{
    public string Content { get; set; } = string.Empty;
    public DateTime? CreationTime { get; set; }                     // No need to add this to the request
}

// Not used in requests
public sealed class CreateActivityLogDto<T> where T : class
{
    public string Content { get; set; } = string.Empty;
    public DateTime? CreationTime { get; set; }
    public string GenericType => typeof(T).Name;
}