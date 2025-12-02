using System.Diagnostics.CodeAnalysis;
using GrisAPI.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IAuthenticationService = GrisAPI.Services.AuthenticationService.IAuthenticationService;

namespace GrisAPI.Controller;

[ApiController]
[ExcludeFromCodeCoverage]
[Route("api/v1/[controller]")]
public sealed class AuthController(IAuthenticationService authenticationService) : ControllerBase
{
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
    {
        var loginResponse = await authenticationService.LoginRequest(loginRequest);

        if (loginResponse.IsBlocked)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        } else if (loginResponse.NameOrPasswordInvalid)
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            loginResponse.UserClaimsPrincipal);

        return Ok();
    }

    [Authorize]
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok();
    }
}