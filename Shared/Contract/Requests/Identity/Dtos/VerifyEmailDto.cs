namespace Contract.Requests.Identity.Dtos;

public sealed class VerifyEmailDto
{
    public string Email { get; init; }
    public string Token { get; init; }
}
