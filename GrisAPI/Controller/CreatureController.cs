using System.Diagnostics.CodeAnalysis;
using GrisAPI.DTOs;
using GrisAPI.Extensions;
using GrisAPI.Services.CreatureService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrisAPI.Controller;

[ApiController]
[ExcludeFromCodeCoverage]
[Route("api/v1/[controller]")]
public sealed class CreatureController(ICreatureService creatureService) : ControllerBase
{
    [Authorize]
    [HttpPost("Create")]
    public async Task<CreatureDto> CreateCreature(string creatureName)
    {
        var userId = this.GetUserId();
        return await creatureService.CreateCreature(creatureName, userId);
    }
}