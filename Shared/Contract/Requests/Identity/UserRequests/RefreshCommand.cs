﻿using Contract.Messaging.Models;
using Contract.Responses.Identity;

namespace Contract.Requests.Identity.UserRequests;

public sealed class RefreshCommand : IRequest<Result<AuthModel>>
{
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; }

    public RefreshCommand(string? accessToken, string? refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}
