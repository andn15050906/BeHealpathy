namespace Contract.Requests.Identity.UserRequests;

public class RequestPasswordResetCommand : IRequest<Result<string>>
{
    public string Email { get; init; }

    public RequestPasswordResetCommand(string email)
    {
        Email = email;
    }
}
