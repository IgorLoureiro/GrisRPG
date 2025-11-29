using System.Security.Claims;
using GrisAPI.DTOs;
using GrisAPI.Helpers.Security;
using GrisAPI.Repositories.UserRepository;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace GrisAPI.Services.AuthenticationService;

public sealed class AuthenticationService(IUserRepository userRepository) : IAuthenticationService
{
    private const int MaxUserAttempts = 5;

    public async Task<LoginResponseDto> LoginRequest(LoginRequestDto loginRequest)
    {
        LoginResponseDto loginResponse = new();
        
        ArgumentException.ThrowIfNullOrEmpty(loginRequest?.Username);

        var user = await userRepository.GetUserByUsernameAsync(loginRequest.Username);

        if (user == null)
        {
            loginResponse.NameOrPasswordInvalid = true;
            return loginResponse;
        } else if (user.IsBlocked)
        {
            loginResponse.IsBlocked = true;
            return loginResponse;
        }

        if (!PasswordHasherHelper.VerifyPassword(loginRequest.Password, user.PasswordHash))
        {
            user.Attempts++;

            if (user.Attempts >= MaxUserAttempts)
                user.IsBlocked = true;
            
            await userRepository.UpdateUserAsync(user);
            loginResponse.NameOrPasswordInvalid = true;
            return loginResponse;
        }

        user.Attempts = 0;
        await userRepository.UpdateUserAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        loginResponse.UserClaimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        return loginResponse;
    }
}