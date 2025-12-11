using System.ComponentModel.DataAnnotations;

namespace GrisAPI.Models;

public class Deck
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    //Join Tables
    public ICollection<Card> Cards { get; set; } = new List<Card>();
    public ICollection<Joker> Jokers { get; set; } = new List<Joker>();
    public ICollection<Creature> Creatures { get; set; } = new List<Creature>();
}