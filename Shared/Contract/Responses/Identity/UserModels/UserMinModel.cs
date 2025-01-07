namespace Contract.Responses.Identity.UserModels;

public sealed class UserMinModel
{
    public Guid Id { get; set; }

    public string FullName { get; set; }
    public string AvatarUrl { get; set; }
}
