using GrisAPI.Models;

namespace GrisAPI.DTOs;

public sealed class ExtraDeckDto
{
    public ExtraDeckDto(){}

    public ExtraDeckDto(ExtraDeck extraDeck)
    {
        Id = extraDeck.Id;
        NumberOfCards = extraDeck.Cards.Count + extraDeck.Jokers.Count;
        Cards = extraDeck.Cards.Select(x => new CardDto(x)).ToList();
        Jokers = extraDeck.Jokers.Select(x => new JokerDto(x)).ToList();
    }
    
    public int Id { get; set; }
    public int NumberOfCards { get; set; } 
    public IEnumerable<CardDto> Cards { get; set; } = new List<CardDto>();
    public IEnumerable<JokerDto> Jokers { get; set; } = new List<JokerDto>();
}