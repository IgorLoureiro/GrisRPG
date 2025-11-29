using System.ComponentModel.DataAnnotations;

namespace GrisAPI.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    public bool IsMaster { get; set; } = false;
    [MaxLength(100)]
    public string PasswordHash { get; set; } = string.Empty;
    public int Attempts { get; set; } = 0;
    public bool IsBlocked { get; set; } = false;
    
    //Join Tables
    public ICollection<Creature> Creatures { get; set; } = new List<Creature>();
}