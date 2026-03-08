using Harmonix.Api.Common.Extensions;
using Harmonix.Api.Features.Auth.Login;
using Harmonix.Api.Features.Auth.Logout;
using Harmonix.Api.Features.Auth.Refresh;
using Microsoft.AspNetCore.Mvc;

namespace Harmonix.Api.Features.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, LoginHandler handler, CancellationToken ct)
    {
        var result = await handler.ExecuteAsync(request, ct);

        if (result.IsFailure)
            return this.GetResult(result);

        var response = result.Value!;

        Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = response.RefreshExpiresAt,
            Path = "/"
        });


        return Ok(new { response.AccessToken, response.AccessExpiresAt });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenHandler handler, CancellationToken ct)
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
            return Unauthorized(new { message = "Refresh token not found" });

        var result = await handler.ExecuteAsync(new RefreshTokenRequest(refreshToken), ct);

        if (!result.IsSuccess)
            return this.GetResult(result);

        var response = result.Value!;

        Response.Cookies.Append("refreshToken", response.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Expires = response.RefreshExpiresAt,
            Path = "/"
        });

        return Ok(new
        {
            accessToken = response.AccessToken,
            expiresAt = response.AccessExpiresAt,
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutHandler handler, CancellationToken ct)
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (!string.IsNullOrEmpty(refreshToken))
        {
            await handler.ExecuteAsync(new LogoutRequest(refreshToken), ct);
        }

        Response.Cookies.Delete("refreshToken", new CookieOptions
        {
            Path = "/",
            Secure = true,
            SameSite = SameSiteMode.None,
            HttpOnly = true
        });

        return Ok();
    }
}
