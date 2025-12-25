using GrisAPI.Models;

namespace GrisAPI.DTOs;

public sealed class DeckDto
{
    public DeckDto(){}

    public DeckDto(Deck deck)
    {
        Id = deck.Id;
        NumberOfCards = deck.Cards.Count + deck.Jokers.Count;
        Name = deck.Name;
        Cards = deck.Cards.Select(c => new CardDto(c)).ToList();
        Jokers = deck.Jokers.Select(j => new JokerDto(j)).ToList();
    }
    
    public int Id { get; set; }
    public int NumberOfCards { get; set; } 
    public string Name { get; set; } = string.Empty;
    public IEnumerable<CardDto> Cards { get; set; } = new List<CardDto>();
    public IEnumerable<JokerDto> Jokers { get; set; } = new List<JokerDto>();
}