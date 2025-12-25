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
    [HttpPost("GetFilteredCreatures")]
    public async Task<ActionResult<CreatureFilterResponse>> GetFilteredCreatures(CreatureFilterRequest filterRequest)
    {
        var userId = this.GetUserId();
        return Ok(await creatureService.GetFilteredCreatures(filterRequest, userId));
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<CreatureDto>> CreateCreature(string creatureName)
    {
        var userId = this.GetUserId();
        return Ok(await creatureService.CreateCreature(creatureName, userId));
    }
    
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CreatureDto>> GetCreatureById(int id)
    {
        var creature = await creatureService.GetCreatureById(id);
        if(creature is null)
            return NotFound();
        
        return Ok(creature);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult> UpdateCreature(CreatureDto creature)
    {
        var wasUpdated = await creatureService.UpdateCreature(creature);
        if(!wasUpdated)
            return NotFound();
        
        return Ok();
    }
    
    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCreature(int id)
    {
        var wasDeleted = await creatureService.DeleteCreatureById(id);
        if (!wasDeleted)
            return NotFound();
        
        return Ok();
    }
}