using System.Diagnostics.CodeAnalysis;
using GrisAPI.DTOs;
using GrisAPI.Services.JokerService;
using Microsoft.AspNetCore.Mvc;

namespace GrisAPI.Controller;

[ApiController]
[ExcludeFromCodeCoverage]
[Route("v1/api/[controller]")]
public class JokerController(IJokerService jokerService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<JokerDto>> GetJokerById(int id)
    {
        var results = await jokerService.GetJokerById(id);
        if(results is null)
            return NotFound();
        
        return Ok(results);
    }

    [HttpPost("GetJokersById")]
    public async Task<ActionResult<List<JokerDto>>> GetJokersById([FromBody] List<int> ids)
    {
        return Ok(await jokerService.GetJokersById(ids));
    }

    [HttpPost("GetJokersByName")]
    public async Task<ActionResult<List<JokerDto>>> GetJokersByName([FromBody] string name)
    {
        return Ok(await jokerService.GetJokersByName(name));
    }

    [HttpPost("CreateJoker")]
    public async Task<ActionResult<JokerDto?>> CreateJoker([FromBody] JokerDto joker)
    {
        var result = await jokerService.AddJoker(joker);
        if (result is null)
            return BadRequest();
        
        return Ok(new JokerDto(result));
    }
    
    [HttpPut]
    public async Task<ActionResult> UpdateJoker([FromBody] JokerDto joker)
    {
        var result = await jokerService.UpdateJoker(joker);
        if (!result)
            return NotFound();
        
        return Ok();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteJoker(int id)
    {
        var result = await jokerService.DeleteJoker(id);
        if (!result)
            return NotFound();
        
        return Ok();
    }
}