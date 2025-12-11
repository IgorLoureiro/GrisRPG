using System.ComponentModel.DataAnnotations;

namespace GrisAPI.Models;

public class Creature
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    //Navigation Property
    public ExtraDeck ExtraDeck { get; set; } = new ExtraDeck();
    
    //Join Tables
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Deck> Decks { get; set; } = new List<Deck>();
}