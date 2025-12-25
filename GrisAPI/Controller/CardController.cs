using System.Diagnostics.CodeAnalysis;
using GrisAPI.DTOs;
using GrisAPI.Services.CardService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrisAPI.Controller;

[ApiController]
[ExcludeFromCodeCoverage]
[Route("api/v1/[controller]")]
public sealed class CardController(ICardService cardService) : ControllerBase
{
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CardDto>> GetCardById(int id)
    {
        var results = await cardService.GetCardById(id);
        if(results is null)
            return NotFound();
        
        return Ok(new CardDto(results));
    }
    
    [Authorize]
    [HttpPost("GetFilteredCards")]
    public async Task<ActionResult<CardFilterResponse>> GetFilteredCards(CardFilterRequest filterRequest)
    {
        var results = await cardService.GetFilteredCards(filterRequest);
        return Ok(results);
    }

    [Authorize]
    [HttpPost("GetCardsById")]
    public async Task<ActionResult<List<CardDto>>> GetCardsById([FromBody] List<int> ids)
    {
        return Ok(await cardService.GetCardsById(ids));
    }

    [Authorize]
    [HttpPost("CreateCard")]
    public async Task<ActionResult<CardDto?>> CreateJoker([FromBody] CardDto card)
    {
        var result = await cardService.AddCard(card);
        if (result is null)
            return BadRequest();
        
        return Ok(new CardDto(result));
    }
    
    [Authorize]
    [HttpPut]
    public async Task<ActionResult> UpdateCard([FromBody] CardDto card)
    {
        var result = await cardService.UpdateCard(card);
        if (!result)
            return NotFound();
        
        return Ok();
    }
    
    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteCard(int id)
    {
        var result = await cardService.DeleteCard(id);
        if (!result)
            return NotFound();
        
        return Ok();
    }
}