using System.ComponentModel.DataAnnotations;

namespace GrisAPI.Models;

public class Joker
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;
    
    //Join Tables
    public ICollection<Deck> Decks { get; set; } = new List<Deck>();
    public ICollection<ExtraDeck> ExtraDecks { get; set; } = new List<ExtraDeck>();
}