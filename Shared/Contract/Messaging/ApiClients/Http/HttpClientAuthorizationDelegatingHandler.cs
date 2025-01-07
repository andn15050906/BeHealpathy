using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace Contract.Messaging.ApiClients.Http;

public sealed class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
{
    private const string ACCESS_TOKEN = "access_token";
    private const string BEARER_SCHEME = "Bearer";

    private readonly IHttpContextAccessor _httpContextAccessor;



    public HttpClientAuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor) : base()
    {
        _httpContextAccessor = httpContextAccessor;
    }



    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var context = _httpContextAccessor.HttpContext;

        if (context is not null)
        {
            var accessToken = await context.GetTokenAsync(ACCESS_TOKEN);
            if (accessToken is not null)
                request.Headers.Authorization = new AuthenticationHeaderValue(BEARER_SCHEME, accessToken);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}