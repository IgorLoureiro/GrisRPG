using System.Diagnostics.CodeAnalysis;
using GrisAPI.DTOs;
using GrisAPI.Services.ExtraDeckService;
using Microsoft.AspNetCore.Mvc;

namespace GrisAPI.Controller;

[ApiController]
[Route("api/v1/[controller]")]
[ExcludeFromCodeCoverage]
public sealed class ExtraDeckController(IExtraDeckService extraDeckService) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ExtraDeckDto?>> GetExtraDeckById(int id)
    {
        var result = await extraDeckService.GetExtraDeckById(id);
        if (result is null)
            return NotFound();
        
        return result;
    }
    
    [HttpPost]
    public async Task<ActionResult> UpdateExtraDeck(ExtraDeckDto extraDeck)
    {
        var result = await extraDeckService.UpdateExtraDeck(extraDeck);
        if (!result)
            return NotFound();
        
        return Ok();
    }
}