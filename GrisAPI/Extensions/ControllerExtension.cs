using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace GrisAPI.Extensions;

public static class ControllerExtension
{
    public static int GetUserId(this ControllerBase controller)
    {
        var userPrincipal = controller.HttpContext.User;

        var userIdClaim = userPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            throw new InvalidOperationException("User ID claim (NameIdentifier) not found in authenticated principal.");
        
        return int.TryParse(userIdClaim, out var userId) 
            ? userId 
            : throw new FormatException("User ID claim invalid.");
    }
}