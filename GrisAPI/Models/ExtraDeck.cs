namespace GrisAPI.Models;

public class ExtraDeck
{
    public int Id { get; set; }
    
    //Navigation Property
    public int CreatureId { get; set; }
    public Creature Creature { get; set; }
    
    //Join Tables
    public ICollection<Card> Cards { get; set; } = new List<Card>();
    public ICollection<Joker> Jokers { get; set; } = new List<Joker>();
}