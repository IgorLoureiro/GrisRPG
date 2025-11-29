using System.Security.Claims;
using GrisAPI.DTOs;

namespace GrisAPI.Services.AuthenticationService;

public interface IAuthenticationService
{
    Task<LoginResponseDto> LoginRequest(LoginRequestDto loginRequest);
}