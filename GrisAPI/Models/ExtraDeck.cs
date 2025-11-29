using System.Collections.ObjectModel;

namespace GrisAPI.Models;

public class ExtraDeck
{
    public int Id { get; set; }
    public int NumberOfCards { get; set; }
    public Creature Creature { get; set; }
    
    //Join Tables
    public ICollection<Card> Cards { get; set; } = new List<Card>();
    public ICollection<Joker> Jokers { get; set; } = new List<Joker>();
}