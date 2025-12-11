using GrisAPI.Models;
using GrisAPI.Models.Enums;

namespace GrisAPI.DTOs;

public sealed class CardDto
{
    public CardDto(){}

    public CardDto(Card card)
    {
        Id = card.Id;
        Name = card.Name;
        Description = card.Description;
        Symbol = card.Symbol;
        Manifestation = card.Manifestation;
    }
    
    public int Id { get; set; }
    public Symbol Symbol { get; set; }
    public Manifestation Manifestation { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Suit => GetSuit(Symbol);

    private static string GetSuit(Symbol symbol)
    {
        return symbol switch
        {
            Symbol.Hearts => "♥",
            Symbol.Clubs => "♣",
            Symbol.Spades => "♠",
            Symbol.Diamonds => "♦",
            _ => string.Empty
        };
    }
}