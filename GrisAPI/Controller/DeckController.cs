using System.Diagnostics.CodeAnalysis;
using GrisAPI.DTOs;
using GrisAPI.Services.DeckService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrisAPI.Controller;

[ApiController]
[ExcludeFromCodeCoverage]
[Route("api/v1/[controller]")]
public sealed class DeckController(IDeckService deckService) : ControllerBase
{
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DeckDto>> GetDeck(int id)
    {
        var result = await deckService.GetDeckById(id);
        if (result is null)
            return NotFound();
        
        return Ok(result);
    }
    
    [Authorize]
    [HttpPost("Creature/{creatureId:int}")]
    public async Task<ActionResult<DeckDto>> AddDeck(DeckDto deck, int creatureId)
    {
        var result = await deckService.AddDeck(deck, creatureId);
        if (result is null)
            return NotFound();
        
        return Ok(result);
    }
    
    [Authorize]
    [HttpPut]
    public async Task<ActionResult<DeckDto>> UpdateDeck(DeckDto deck)
    {
        var result = await deckService.UpdateDeck(deck);
        if (!result)
            return NotFound();
        
        return Ok(result);
    }
    
    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<DeckDto>> DeleteDeck(int id)
    {
        var result = await deckService.DeleteDeck(id);
        if (!result)
            return NotFound();
        
        return Ok(result);
    }
}