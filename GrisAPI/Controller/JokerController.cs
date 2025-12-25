using System.Diagnostics.CodeAnalysis;
using GrisAPI.DTOs;
using GrisAPI.Services.JokerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrisAPI.Controller;

[ApiController]
[ExcludeFromCodeCoverage]
[Route("v1/api/[controller]")]
public sealed class JokerController(IJokerService jokerService) : ControllerBase
{
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<JokerDto>> GetJokerById(int id)
    {
        var results = await jokerService.GetJokerById(id);
        if(results is null)
            return NotFound();
        
        return Ok(results);
    }

    [Authorize]
    [HttpPost("GetJokersById")]
    public async Task<ActionResult<List<JokerDto>>> GetJokersById([FromBody] List<int> ids)
    {
        return Ok(await jokerService.GetJokersById(ids));
    }

    [Authorize]
    [HttpGet("GetJokersByName/{name}")]
    public async Task<ActionResult<List<JokerDto>>> GetJokersByName(string name)
    {
        return Ok(await jokerService.GetJokersByName(name));
    }

    [Authorize]
    [HttpPost("CreateJoker")]
    public async Task<ActionResult<JokerDto?>> CreateJoker([FromBody] JokerDto joker)
    {
        var result = await jokerService.AddJoker(joker);
        if (result is null)
            return BadRequest();
        
        return Ok(new JokerDto(result));
    }
    
    [Authorize]
    [HttpPut]
    public async Task<ActionResult> UpdateJoker([FromBody] JokerDto joker)
    {
        var result = await jokerService.UpdateJoker(joker);
        if (!result)
            return NotFound();
        
        return Ok();
    }
    
    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteJoker(int id)
    {
        var result = await jokerService.DeleteJoker(id);
        if (!result)
            return NotFound();
        
        return Ok();
    }
}