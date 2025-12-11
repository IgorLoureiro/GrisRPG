using GrisAPI.Models;

namespace GrisAPI.DTOs;

public sealed class JokerDto
{
    public JokerDto(){}

    public JokerDto(Joker joker)
    {
        Id = joker.Id;
        Name = joker.Name;
        Description = joker.Description;
    }
    
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}