using System.Security.Claims;

namespace GrisAPI.DTOs;

public class LoginResponseDto
{
    public ClaimsPrincipal UserClaimsPrincipal { get; set; } = null!;
    public bool NameOrPasswordInvalid { get; set; } = false;
    public bool IsBlocked { get; set; } = false;
}