using GrisAPI.Models;

namespace GrisAPI.DTOs;

public class CreatureDto
{
    public CreatureDto(){}
    
    public CreatureDto(Creature creature)
    {
        Id = creature.Id;
        Name = creature.Name;
        ExtraDeck = new ExtraDeckDto(creature.ExtraDeck);
    }
    
    public int Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public ExtraDeckDto ExtraDeck { get; set; } = new ExtraDeckDto();
}