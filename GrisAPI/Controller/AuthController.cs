using System.Diagnostics.CodeAnalysis;
using GrisAPI.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using IAuthenticationService = GrisAPI.Services.AuthenticationService.IAuthenticationService;

namespace GrisAPI.Controller;

[ApiController]
[ExcludeFromCodeCoverage]
[Route("Gris/[controller]")]
public sealed class AuthController(IAuthenticationService authenticationService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto loginRequest)
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
}