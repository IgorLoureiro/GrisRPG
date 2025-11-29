using System.ComponentModel.DataAnnotations;
using GrisAPI.Models.Enums;

namespace GrisAPI.Models;

public class Card
{
    public int Id { get; set; }
    public Symbol Symbol { get; set; }
    public Manifestation Manifestation { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;
    
    //Join Tables
    public ICollection<Deck> Decks { get; set; } = new List<Deck>();
    public ICollection<ExtraDeck> ExtraDecks { get; set; } = new List<ExtraDeck>();
}