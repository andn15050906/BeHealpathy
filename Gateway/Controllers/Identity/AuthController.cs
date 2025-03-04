using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Gateway.Helpers.AppStart;
using Contract.Messaging.ApiClients.Http;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Infrastructure.Helpers.Email;
using Contract.Helpers.AppExploration;
using Contract.Responses.Identity;
using Contract.Helpers.FeatureFlags;
using Contract.Requests.Identity.UserRequests.Dtos;
using Contract.Requests.Identity.UserRequests;

namespace Gateway.Controllers.Identity;

public sealed class AuthController : ContractController
{
    public AuthController(IMediator mediator) : base(mediator) { }



    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn([FromBody] SignInDto dto)
    {
        SignInCommand command = new(dto);

        var result = await _mediator.Send(command);
        if (result.IsSuccessful)
            SetAuthData(result.Data!);
        return result.AsResponse();
    }

    [HttpGet("google-oauth")]
    public IActionResult SignInWithGoogle()
    {
        return Challenge(
            new AuthenticationProperties() { RedirectUri = $"/api/auth/oauth-callback" },
            GoogleDefaults.AuthenticationScheme
        );
    }

    [HttpGet("oauth-callback")]
    public async Task<IActionResult> OAuthCallback([FromServices] IOptions<AppInfoOptions> appInfo)
    {
        // Make database and claims be in a valid state
        var command = new ExternalSignInCommand(User);
        var result = await _mediator.Send(command);

        if (result.Data is null)
            return result.AsResponse();

        // After this, there will be two NameIdentifier Claims
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, result.Data.Principle!);

        //string data = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions
        //{
        //    ReferenceHandler = ReferenceHandler.IgnoreCycles
        //});
        string data = JsonSerializer.Serialize(result.Data.User);
        return Redirect($"{appInfo.Value.MainFrontendApp}/?external_redirect={data}");
    }

    [HttpPost("SignOut")]
    public new void SignOut()
    {
        SetCredentials(string.Empty, string.Empty, Configurer.GetExpiredOptions());
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    [HttpPost("Refresh")]
    public async Task<IActionResult> Refresh([FromServices] ITokenService tokenService)
    {
        RefreshCommand command = new(GetAccessToken(), GetRefreshToken());

        var result = await _mediator.Send(command);
        if (result.IsSuccessful)
            SetAuthData(result.Data!);
        return result.AsResponse();
    }

    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> RequestPasswordResetAsync(
        [FromBody] string email, [FromServices] EmailService emailService,
        [FromServices] IOptions<AppInfoOptions> appInfo, [FromServices] IOptions<FeatureFlagOptions> flags)
    {
        if (!flags.Value.EmailEnabled)
            return Ok(BusinessMessages.FEATURE_DISABLED);

        RequestPasswordResetCommand command = new(email);

        var result = await _mediator.Send(command);
        if (!result.IsSuccessful)
            return result.AsResponse();

        string link = $"{appInfo.Value.MainFrontendApp}/reset-password/{email}/{result.Data}";
#pragma warning disable CS4014
        emailService.SendPasswordResetEmail(email, link);
#pragma warning restore CS4014
        return result.AsResponse();
    }

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        ResetPasswordCommand command = new(dto);
        return await Send(command);
    }






    private void SetAuthData(AuthModel authData)
    {
        SetCredentials(authData.AccessToken, authData.RefreshToken, Configurer.GetAuthOptions());
    }

    private string? GetAccessToken() => Request.Cookies[HttpContextExtensions.BEARER];

    private string? GetRefreshToken() => Request.Cookies[HttpContextExtensions.REFRESH];

    private void SetCredentials(string accessToken, string refreshToken, CookieOptions options)
    {
        options.HttpOnly = true;
        Response.Cookies.Append(HttpContextExtensions.BEARER, accessToken, options);
        Response.Cookies.Append(HttpContextExtensions.REFRESH, refreshToken, options);
    }
}